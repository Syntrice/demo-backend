using DemoBackend.Models.UserAccounts.Requests;
using FluentValidation;

namespace DemoBackend.Models.UserAccounts.Validators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(model => model.RefreshToken).NotEmpty();
    }
}