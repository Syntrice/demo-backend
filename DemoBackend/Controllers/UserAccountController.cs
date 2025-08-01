using DemoBackend.Common.Results;
using DemoBackend.Models.UserAccounts.Requests;
using DemoBackend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAccountController(
    IUserAccountService accountService,
    IValidator<RegisterRequest> registerRequestValidator,
    IValidator<LoginRequest> logingRequestValidator,
    IValidator<RefreshRequest> refreshRequestValidator,
    IValidator<RevokeRefreshTokenFamilyRequest> revokeRefreshTokenFamilyRequestValidator)
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
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var validationResult = await logingRequestValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await accountService.LoginAsync(model);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model)
    {
        var validationResult = await refreshRequestValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await accountService.RefreshAsync(model);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        return Ok(result.Value);
    }

    [HttpPost("revokeRefreshTokenFamily")]
    [Authorize]
    public async Task<IActionResult> RevokeRefreshTokenFamily()
    {
        var familyId = User.FindFirst("refreshTokenFamily")?.Value;

        var revokeRequest = new RevokeRefreshTokenFamilyRequest
            { RefreshTokenFamilyId = new Guid(familyId) };
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