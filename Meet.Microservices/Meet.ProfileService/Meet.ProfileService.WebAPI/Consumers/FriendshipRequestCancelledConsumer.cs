using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Meet.ProfileService.WebAPI.Consumers;

public class FriendshipRequestCancelledConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;

    public FriendshipRequestCancelledConsumer(IConfiguration configuration, ILogger<FriendshipRequestCancelledConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-FriendshipRequestCancelled", configuration, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<FriendshipRequestCancelled>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error.");
            return;
        }
        var canceller = dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.cancellerId);
        var sender = dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.friendId);
        if (canceller == null || sender == null)
        {
            _logger.LogError("Canceller or sender is not found.");
            return;
        }
        if ((sender.SentFriendshipRequests.Contains(canceller) && !canceller.ReceivedFriendshipRequests.Contains(sender))
        || (!sender.SentFriendshipRequests.Contains(canceller) && canceller.ReceivedFriendshipRequests.Contains(sender))
        || (sender.Friends.Contains(canceller) && !canceller.Friends.Contains(sender))
        || (!sender.Friends.Contains(canceller) && canceller.Friends.Contains(sender)))
        {
            _logger.LogError("There is a problem with this friendship. SenderId: {senderId}, CancellerId: {cancellerId}", sender.Id, canceller.Id);
            return;
        }
        if (!canceller.ReceivedFriendshipRequests.Contains(sender) || !sender.SentFriendshipRequests.Contains(canceller))
        {
            _logger.LogWarning("There is no such friendship request. SenderId: {senderId}, CancellerId: {cancellerId}", sender.Id, canceller.Id);
            return;
        }
        if (sender.Friends.Contains(canceller) && canceller.Friends.Contains(sender))
        {
            _logger.LogWarning("Friendship already continues. SenderId: {senderId}, CancellerId: {cancellerId}", sender.Id, canceller.Id);
            return;
        }
        canceller.SentFriendshipRequests.Remove(sender);
        sender.ReceivedFriendshipRequests.Remove(canceller);
        dbContext.SaveChanges();
        _logger.LogInformation("Friendship request cancelled.");
    }
}
