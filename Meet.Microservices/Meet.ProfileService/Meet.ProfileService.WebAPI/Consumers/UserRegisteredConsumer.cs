using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;

namespace Meet.ProfileService.WebAPI.Consumers;

public class UserRegisteredConsumer : IConsumer
{
    public string QueueName { get; set; } = "Meet-UserRegistered";

    private readonly ApplicationDbContext _dbContext;

    public UserRegisteredConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ConsumeMessage(string message)
    {
        var @object = JsonSerializer.Deserialize<UserRegistered>(message);
        if (@object == null)
            return;
        var item = _dbContext.Profiles.FirstOrDefault(x => x.Id == @object.id);
        if (item != null)
            return;
        _dbContext.Profiles.Add(new()
        {
            Id = @object.id,
            FirstName = @object.firstName,
            LastName = @object.lastName
        });
        _dbContext.SaveChanges();
    }
}
