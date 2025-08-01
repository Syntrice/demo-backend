using DemoBackend.Models.UserAccounts.Requests;
using FluentValidation;

namespace DemoBackend.Models.UserAccounts.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(model => model.Email).NotEmpty().EmailAddress();
        RuleFor(model => model.Password).NotEmpty();
    }
}