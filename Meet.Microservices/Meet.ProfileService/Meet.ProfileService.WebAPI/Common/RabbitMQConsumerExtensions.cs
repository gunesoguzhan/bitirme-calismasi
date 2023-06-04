using Meet.ProfileService.WebAPI.Consumers;

namespace Meet.ProfileService.WebAPI.Common;

public static class RabbitMQConsumerExtensions
{
    public static IServiceCollection AddRabbitMQConsumers(this IServiceCollection services)
    {
        services.AddHostedService<UserRegisteredConsumer>();
        services.AddHostedService<FriendshipRemovedConsumer>();
        services.AddHostedService<FriendshipRequestAcceptedConsumer>();
        services.AddHostedService<FriendshipRequestCancelledConsumer>();
        services.AddHostedService<FriendshipRequestRejectedConsumer>();
        services.AddHostedService<FriendshipRequestSentConsumer>();
        return services;
    }
}