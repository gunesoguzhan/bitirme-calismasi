using FluentValidation;
using Meet.IdentityService.WebAPI.Dtos;
using Meet.IdentityService.WebAPI.Features.Validation;

namespace Meet.IdentityService.WebAPI.Common;

static class FluentValidationExtension
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IValidator<LoginUserDto>, LoginUserDtoValidator>();
        serviceCollection.AddSingleton<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
        return serviceCollection;
    }
}