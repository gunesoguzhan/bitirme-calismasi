using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.RoomService.WebAPI.Contracts;
using Meet.RoomService.WebAPI.DataAccess;

namespace Meet.RoomService.WebAPI.Consumers;

public class MessageReceivedConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MessageReceivedConsumer> _logger;
    public MessageReceivedConsumer(IConfiguration configuration, ILogger<MessageReceivedConsumer> logger, IServiceScopeFactory scopeFactory)
    : base("Meet-MessageReceived", configuration, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<MessageReceived>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error");
            return;
        }
        var user = dbContext.Users.FirstOrDefault(x => x.Id == @object.userId);
        if (user == null)
        {
            _logger.LogError("User not found. UserId: {userId}", @object.userId);
            return;
        }
        var room = dbContext.Rooms.FirstOrDefault(x => x.Id == @object.roomId);
        if (room == null)
        {
            _logger.LogError("Room not found. RoomId: {roomId}, UserId: {userId}", @object.roomId, @object.userId);
            return;
        }
        dbContext.Messages.Add(new()
        {
            MessageText = @object.messageText,
            Date = @object.date,
            User = user,
            Room = room
        });
        dbContext.SaveChanges();
        _logger.LogInformation("Message saved. RoomId: {roomId}, UserId: {userId}", @object.roomId, @object.userId);
    }
}