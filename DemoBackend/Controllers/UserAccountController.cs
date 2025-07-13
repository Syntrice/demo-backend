using DemoBackend.Common.Results;
using DemoBackend.Models.UserAccounts.Requests;
using DemoBackend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAccountController(
    IUserAccountService accountService,
    IValidator<RegisterRequest> registerRequestValidator) : ControllerBase
{
    [HttpPost]
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
}