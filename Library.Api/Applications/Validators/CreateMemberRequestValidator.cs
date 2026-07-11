using FluentValidation;
using Library.Api.Contracts.Members;

namespace Library.Api.Applications.Validators;

public class CreateMemberRequestValidator
    : AbstractValidator<CreateMemberRequest>
{
    public CreateMemberRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}