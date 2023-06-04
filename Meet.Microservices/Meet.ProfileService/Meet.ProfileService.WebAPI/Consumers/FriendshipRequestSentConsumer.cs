using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Meet.ProfileService.WebAPI.Consumers;

public class FriendshipRequestSentConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;

    public FriendshipRequestSentConsumer(IConfiguration configuration, ILogger<FriendshipRequestSentConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-FriendshipRequestSent", configuration, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<FriendshipRequestSent>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error");
            return;
        }
        var sender = dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.senderId);
        var receiver = dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.receiverId);
        if (sender == null || receiver == null)
        {
            _logger.LogError("Sender or receiver is not found.");
            return;
        }
        if ((sender.SentFriendshipRequests.Contains(receiver) && !receiver.ReceivedFriendshipRequests.Contains(sender))
        || (!sender.SentFriendshipRequests.Contains(receiver) && receiver.ReceivedFriendshipRequests.Contains(sender))
        || (sender.Friends.Contains(receiver) && !receiver.Friends.Contains(sender))
        || (!sender.Friends.Contains(receiver) && receiver.Friends.Contains(sender)))
        {
            _logger.LogError("There is a problem with this friendship. SenderId: {senderId}, ReceiverId: {receiverId}", sender.Id, receiver.Id);
            return;
        }
        if (sender.SentFriendshipRequests.Contains(receiver) && receiver.ReceivedFriendshipRequests.Contains(sender))
        {
            _logger.LogWarning("Friendship request already sended. SenderId: {senderId}, ReceiverId: {receiverId}", sender.Id, receiver.Id);
            return;
        }
        if (sender.Friends.Contains(receiver) && receiver.Friends.Contains(sender))
        {
            _logger.LogWarning("Friendship already continues. SenderId: {senderId}, ReceiverId: {receiverId}", sender.Id, receiver.Id);
            return;
        }
        if (sender.ReceivedFriendshipRequests.Contains(receiver) && receiver.SentFriendshipRequests.Contains(sender))
        {
            receiver.SentFriendshipRequests.Remove(sender);
            sender.ReceivedFriendshipRequests.Remove(receiver);
            sender.Friends.Add(receiver);
            receiver.Friends.Add(sender);
            dbContext.SaveChanges();
            _logger.LogInformation("Friendship request accepted. SenderId: {senderId}, ReceiverId: {receiverId}", sender.Id, receiver.Id);
            return;
        }
        sender.SentFriendshipRequests.Add(receiver);
        receiver.ReceivedFriendshipRequests.Add(sender);
        dbContext.SaveChanges();
        _logger.LogInformation("Friendship request sended. SenderId: {senderId}, ReceiverId: {receiverId}", sender.Id, receiver.Id);
    }
}
