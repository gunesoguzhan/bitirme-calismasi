using System.Text.Json;
using Meet.Common.RabbitMQ;

namespace Meet.IdentityService.WebAPI.Producers;

public class UserRegisteredProducer : RabbitMQProducerBase
{
    public UserRegisteredProducer(IConfiguration configuration, ILogger<RabbitMQProducerBase> logger) : base(configuration, logger)
    {
    }

    public void SendMessage<T>(T @object)
    {
        PublishMessage("Meet-UserRegistered", JsonSerializer.Serialize(@object));
    }
}
