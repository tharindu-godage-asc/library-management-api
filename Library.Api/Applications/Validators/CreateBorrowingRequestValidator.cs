using FluentValidation;
using Library.Api.Contracts.Borrowings;

namespace Library.Api.Applications.Validators;

public class CreateBorrowingRequestValidator
    : AbstractValidator<CreateBorrowingRequest>
{
    public CreateBorrowingRequestValidator()
    {
        RuleFor(x => x.BookId)
            .GreaterThan(0);

        RuleFor(x => x.MemberId)
            .GreaterThan(0);
    }
}