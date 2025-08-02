using DemoBackend.Common.Results;
using DemoBackend.Models.UserAccounts.Requests;
using DemoBackend.Services;
using DemoBackend.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DemoBackend.Controllers;

[ApiController]
[Route("api/users")]
public class UserAccountController(
    IUserAccountService accountService,
    IValidator<RegisterRequest> registerRequestValidator,
    IValidator<LoginRequest> logingRequestValidator,
    IValidator<RefreshRequest> refreshRequestValidator,
    IValidator<RevokeRefreshTokenFamilyRequest> revokeRefreshTokenFamilyRequestValidator,
    IOptions<JWTSettings> jwtSettings)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var validationResult = await registerRequestValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await accountService.RegisterAsync(model);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        var created = result.Value;
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model,
        [FromQuery] bool useCookies = false)
    {
        var validationResult = await logingRequestValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await accountService.LoginAsync(model);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);

        if (useCookies)
        {
            // Set cookies for both access token and refresh token
            Response.Cookies.Append("access-token", result.Value.AccessToken, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(jwtSettings.Value.ExpirationInMinutes),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                // unfortunately we have to use SameSiteMode.None as this is an API and will be called by a separate
                // front end server likely on a different domain. 
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Append("refresh-token", result.Value.RefreshToken, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value
                    .RefreshTokenExpirationInDays),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                // unfortunately we have to use SameSiteMode.None as this is an API and will be called by a separate
                // front end server likely on a different domain. 
                SameSite = SameSiteMode.None
            });
            return Ok();
        }

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model,
        [FromQuery] bool useCookies = false)
    {
        if (useCookies)
        {
            var refreshTokenCookie = Request.Cookies["refresh-token"]; // attempt to get cookie

            if (string.IsNullOrEmpty(refreshTokenCookie)) // check if cookie exists
            {
                return BadRequest();
            }

            var request = new RefreshRequest
            {
                RefreshToken = refreshTokenCookie
            };

            var result = await accountService.RefreshAsync(request);
            if (result.IsFailure) return result.ToProblemDetailsResponse(this);

            Response.Cookies.Append("access-token", result.Value.AccessToken, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(jwtSettings.Value.ExpirationInMinutes),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                // unfortunately we have to use SameSiteMode.None as this is an API and will be called by a separate
                // front end server likely on a different domain. 
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Append("refresh-token", result.Value.RefreshToken, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value
                    .RefreshTokenExpirationInDays),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                // unfortunately we have to use SameSiteMode.None as this is an API and will be called by a separate
                SameSite = SameSiteMode.None
            });

            return Ok();
        }
        else
        {
            var validationResult = await refreshRequestValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return validationResult.ToProblemDetailsResponse(this);
            }

            var result = await accountService.RefreshAsync(model);
            return result.IsFailure ? result.ToProblemDetailsResponse(this) : Ok(result.Value);
        }
    }

    [HttpPost("revoke-refresh-token-family")]
    [Authorize]
    public async Task<IActionResult> RevokeRefreshTokenFamily()
    {
        var familyId = User.FindFirst("refresh_token_family")?.Value;

        if (!Guid.TryParse(familyId, out var parsedGuid))
        {
            return BadRequest("Invalid refresh token family ID in token claims.");
        }

        var revokeRequest = new RevokeRefreshTokenFamilyRequest
            { RefreshTokenFamilyId = parsedGuid };

        var validationResult =
            await revokeRefreshTokenFamilyRequestValidator.ValidateAsync(revokeRequest);
        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await accountService.RevokeRefreshTokenFamilyAsync(revokeRequest);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        return Ok();
    }
}