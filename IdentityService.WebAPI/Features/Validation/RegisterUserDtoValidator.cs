using FluentValidation;
using IdentityService.WebAPI.Dtos;

namespace IdentityService.WebAPI.Features.Validation;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.username)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(30);

        RuleFor(x => x.email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(360);

        RuleFor(x => x.password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.firstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.lastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}