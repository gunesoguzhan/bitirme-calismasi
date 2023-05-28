using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;

namespace Meet.ProfileService.WebAPI.Consumers;

public class UserRegisteredConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserRegisteredConsumer(IConfiguration configuration, ILogger<UserRegisteredConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-UserRegistered", configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<UserRegistered>(message);
        if (@object == null) return;
        dbContext.Profiles.Add(new()
        {
            Id = @object.id,
            FirstName = @object.firstName,
            LastName = @object.lastName
        });
        dbContext.SaveChanges();
    }
}
