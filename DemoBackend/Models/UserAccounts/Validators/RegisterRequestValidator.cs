using DemoBackend.Models.UserAccounts.Requests;
using FluentValidation;

namespace DemoBackend.Models.UserAccounts.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(model => model.Email).NotEmpty().EmailAddress();
        RuleFor(model => model.Password).NotEmpty();
        RuleFor(model => model.DisplayName).NotEmpty();
    }
}