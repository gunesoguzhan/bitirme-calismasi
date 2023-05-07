using FluentValidation;
using IdentityService.WebAPI.Dtos;

namespace IdentityService.WebAPI.Features.Validation;

class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.usernameOrEmail)
            .NotEmpty()
            .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$|^[\w\d]+$")
            .WithMessage("Username or e-mail must be valid.")
            .MinimumLength(4)
            .MaximumLength(360);

        RuleFor(x => x.password)
            .NotEmpty()
            .MinimumLength(6);
    }
}