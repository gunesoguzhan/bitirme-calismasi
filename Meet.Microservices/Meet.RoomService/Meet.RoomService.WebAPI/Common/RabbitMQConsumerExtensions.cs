using Meet.RoomService.WebAPI.Consumers;

namespace Meet.RoomService.WebAPI.Common;

public static class RabbitMQConsumerExtensions
{
    public static IServiceCollection AddRabbitMQConsumers(this IServiceCollection services)
    {
        services.AddHostedService<MessageReceivedConsumer>();
        return services;
    }
}