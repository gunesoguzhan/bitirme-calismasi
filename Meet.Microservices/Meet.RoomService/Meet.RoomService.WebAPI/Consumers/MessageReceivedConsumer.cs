using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.Contracts;
using Meet.RoomService.WebAPI.DataAccess;

namespace Meet.RoomService.WebAPI.Consumers;

public class MessageReceivedConsumer : IConsumer
{
    public string QueueName { get; set; } = "Meet-MessageReceived";

    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MessageReceivedConsumer> _logger;

    public MessageReceivedConsumer(ApplicationDbContext dbContext, ILogger<MessageReceivedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void ConsumeMessage(string message)
    {
        var @object = JsonSerializer.Deserialize<MessageReceived>(message);
        if (@object == null)
            return;
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == @object.userId);
        var room = _dbContext.Rooms.FirstOrDefault(x => x.Id == @object.roomId);
        if (user == null || room == null)
            return;
        _dbContext.Messages.Add(
        new()
        {
            MessageText = @object.messageText,
            DateTime = @object.dateTime,
            User = user,
            Room = room
        });
        _dbContext.SaveChanges();
    }
}