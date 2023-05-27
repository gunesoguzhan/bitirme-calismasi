using Meet.Common.RabbitMQ;
using Meet.RoomService.WebAPI.Consumers;

namespace Meet.RoomService.WebAPI.Common;

public static class ConsumerExtensions
{
    public static IServiceCollection AddRabbitMQConsumers(this IServiceCollection services)
    {
        services.AddScoped<IConsumer, MessageReceivedConsumer>();
        return services;
    }
}