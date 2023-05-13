using FluentValidation;
using Meet.ProfileService.WebAPI.Dtos;
using Meet.ProfileService.WebAPI.Features.Validation;

namespace Meet.ProfileService.WebAPI.Common;

static class FluentValidationExtension
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IValidator<SendFriendshipRequestDto>, SendFriendshipRequestDtoValidator>();
        serviceCollection.AddSingleton<IValidator<AcceptFriendshipRequestDto>, AcceptFriendshipRequestDtoValidator>();
        serviceCollection.AddSingleton<IValidator<RejectFriendshipRequestDto>, RejectFriendshipRequestDtoValidator>();
        serviceCollection.AddSingleton<IValidator<CancelFriendshipRequestDto>, CancelFriendshipRequestDtoValidator>();
        serviceCollection.AddSingleton<IValidator<RemoveFriendshipDto>, RemoveFriendshipDtoValidator>();
        return serviceCollection;
    }
}