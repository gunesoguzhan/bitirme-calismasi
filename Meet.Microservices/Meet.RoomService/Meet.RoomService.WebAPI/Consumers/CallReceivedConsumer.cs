using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.RoomService.WebAPI.Contracts;
using Meet.RoomService.WebAPI.DataAccess;

namespace Meet.RoomService.WebAPI.Consumers;

public class CallReceivedConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CallReceivedConsumer> _logger;

    public CallReceivedConsumer(IConfiguration configuration, ILogger<CallReceivedConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-CallReceived", configuration, logger)
    {
        _logger = logger;
        this._scopeFactory = scopeFactory;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<CallReceived>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error");
            return;
        }
        var user = dbContext.Users.FirstOrDefault(x => x.Id == @object.callerId);
        if (user == null)
        {
            _logger.LogError("User not found. UserId: {userId}", @object.callerId);
            return;
        }
        var room = dbContext.Rooms.FirstOrDefault(x => x.Id == @object.roomId);
        if (room == null)
        {
            _logger.LogError("Room not found. RoomId: {roomId}, UserId: {userId}", @object.roomId, @object.callerId);
            return;
        }
        dbContext.Calls.Add(new()
        {
            Date = @object.date,
            Room = room,
            Caller = user
        });
        dbContext.SaveChanges();
        _logger.LogInformation("Call saved. RoomId: {roomId}, UserId: {userId}", @object.roomId, @object.callerId);
    }
}