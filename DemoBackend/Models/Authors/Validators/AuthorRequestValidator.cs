using DemoBackend.Models.Authors.Requests;
using FluentValidation;

namespace DemoBackend.Models.Authors.Validators;

public class AuthorRequestValidator : AbstractValidator<AuthorRequestModel>
{
    public AuthorRequestValidator()
    {
        RuleFor(model => model.Name).NotEmpty();
    }
}