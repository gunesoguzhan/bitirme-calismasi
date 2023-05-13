using FluentValidation;
using Meet.IdentityService.WebAPI.Dtos;

namespace Meet.IdentityService.WebAPI.Features.Validation;

class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.usernameOrEmail)
            .NotEmpty().WithMessage("Required.")
            .Matches(@"^[a-zA-Z0-9_-]{3,30}$|^([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+.[a-zA-Z]{2,})$")
            .WithMessage("Enter a valid username or email.");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("Required.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
            .WithMessage("Enter one letter, one number and 8 characters at least.");
    }
}