using DemoBackend.Models.UserAccounts.Requests;
using FluentValidation;

namespace DemoBackend.Models.UserAccounts.Validators;

public class
    RevokeRefreshTokenFamilyRequestValidator : AbstractValidator<RevokeRefreshTokenFamilyRequest>
{
    public RevokeRefreshTokenFamilyRequestValidator()
    {
        RuleFor(model => model.RefreshTokenFamilyId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}