using FluentValidation;
using Library.Api.Contracts.Books;

namespace Library.Api.Applications.Validators;

public class CreateBookRequestValidator
    : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Author)
            .NotEmpty();

        RuleFor(x => x.Isbn)
            .NotEmpty();

        RuleFor(x => x.PublishedYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.TotalCopies)
            .GreaterThan(0);
    }
}