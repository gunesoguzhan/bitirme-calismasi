using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Meet.ProfileService.WebAPI.Consumers;

public class FriendshipRequestAcceptedConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;

    public FriendshipRequestAcceptedConsumer(IConfiguration configuration, ILogger<FriendshipRequestAcceptedConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-FriendshipRequestAccepted", configuration, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<FriendshipRequestAccepted>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error.");
            return;
        }
        var accepter = dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.accepterId);
        var sender = dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.senderId);
        if (accepter == null || sender == null)
        {
            _logger.LogError("Accepter or receiver is not found.");
            return;
        }
        if ((sender.SentFriendshipRequests.Contains(accepter) && !accepter.ReceivedFriendshipRequests.Contains(sender))
        || (!sender.SentFriendshipRequests.Contains(accepter) && accepter.ReceivedFriendshipRequests.Contains(sender))
        || (sender.Friends.Contains(accepter) && !accepter.Friends.Contains(sender))
        || (!sender.Friends.Contains(accepter) && accepter.Friends.Contains(sender)))
        {
            _logger.LogError("There is a problem with this friendship. SenderId: {senderId}, AccepterId: {accepterId}", sender.Id, accepter.Id);
            return;
        }
        if (!accepter.ReceivedFriendshipRequests.Contains(sender) || !sender.SentFriendshipRequests.Contains(accepter))
        {
            _logger.LogWarning("There is no such friendship request. SenderId: {senderId}, AccepterId: {accepterId}", sender.Id, accepter.Id);
            return;
        }
        if (sender.Friends.Contains(accepter) && accepter.Friends.Contains(sender))
        {
            _logger.LogWarning("Friendship already continues. SenderId: {senderId}, AccepterId: {accepterId}", sender.Id, accepter.Id);
            return;
        }
        accepter.ReceivedFriendshipRequests.Remove(sender);
        sender.SentFriendshipRequests.Remove(accepter);
        accepter.Friends.Add(sender);
        sender.Friends.Add(accepter);
        dbContext.SaveChanges();
        _logger.LogInformation("Friendship request accepted. SenderId: {senderId}, AccepterId: {accepterId}", sender.Id, accepter.Id);
    }
}
