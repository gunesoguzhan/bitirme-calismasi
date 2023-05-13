using FluentValidation;
using Meet.IdentityService.WebAPI.Dtos;

namespace Meet.IdentityService.WebAPI.Features.Validation;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.firstName)
            .NotEmpty().WithMessage("Required.")
            .Matches(@"^[a-zA-Z]{2,30}$").WithMessage("Enter a valid name.");

        RuleFor(x => x.lastName)
            .NotEmpty().WithMessage("Required.")
            .Matches(@"^[a-zA-Z]{2,30}$").WithMessage("Enter a valid name.");

        RuleFor(x => x.username)
            .NotEmpty().WithMessage("Required.")
            .Matches(@"^[a-zA-Z0-9_-]{3,30}$").WithMessage("Enter a valid username.");

        RuleFor(x => x.email)
            .NotEmpty().WithMessage("Required.")
            .EmailAddress().WithMessage("Enter a valid email.");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("Required.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
            .WithMessage("Enter one letter, one number and 8 characters at least.");

        RuleFor(x => x.confirmPassword)
            .NotEmpty().WithMessage("Required.")
            .Equal(x => x.password).WithMessage("Passwords do not match.");
    }
}