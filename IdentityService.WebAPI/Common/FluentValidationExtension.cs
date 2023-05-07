using FluentValidation;
using IdentityService.WebAPI.Dtos;
using IdentityService.WebAPI.Features.Validation;

namespace IdentityService.WebAPI.Common;

static class FluentValidationExtension
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IValidator<LoginUserDto>, LoginUserDtoValidator>();
        serviceCollection.AddSingleton<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
        return serviceCollection;
    }
}