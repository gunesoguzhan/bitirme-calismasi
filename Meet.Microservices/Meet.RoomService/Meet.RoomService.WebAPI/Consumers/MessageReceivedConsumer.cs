using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.Contracts;
using Meet.RoomService.WebAPI.DataAccess;

namespace Meet.RoomService.WebAPI.Consumers;

public class MessageReceivedConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MessageReceivedConsumer(IConfiguration configuration, ILogger<MessageReceivedConsumer> logger, IServiceScopeFactory scopeFactory)
    : base("Meet-MessageReceived", configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<MessageReceived>(message);
        if (@object == null)
            return;
        var user = dbContext.Users.FirstOrDefault(x => x.Id == @object.userId);
        var room = dbContext.Rooms.FirstOrDefault(x => x.Id == @object.roomId);
        if (user == null || room == null)
            return;
        dbContext.Messages.Add(
        new()
        {
            MessageText = @object.messageText,
            Date = @object.date,
            User = user,
            Room = room
        });
        dbContext.SaveChanges();
    }
}