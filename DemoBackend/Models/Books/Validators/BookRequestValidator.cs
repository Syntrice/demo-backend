using DemoBackend.Models.Books.Requests;
using FluentValidation;

namespace DemoBackend.Models.Books.Validators;

public class BookRequestValidator : AbstractValidator<BookRequestModel>
{
    public BookRequestValidator()
    {
        RuleFor(model => model.Title).NotEmpty();
    }
}