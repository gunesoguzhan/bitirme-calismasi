using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Meet.ProfileService.WebAPI.Consumers;

public class FriendshipRequestRejectedConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;

    public FriendshipRequestRejectedConsumer(IConfiguration configuration, ILogger<FriendshipRequestRejectedConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-FriendshipRequestRejected", configuration, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<FriendshipRequestRejected>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error");
            return;
        }
        var rejecter = dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.rejecterId);
        var sender = dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.senderId);
        if (rejecter == null || sender == null)
        {
            _logger.LogError("Rejecter or receiver is not found.");
            return;
        }
        if ((sender.SentFriendshipRequests.Contains(rejecter) && !rejecter.ReceivedFriendshipRequests.Contains(sender))
        || (!sender.SentFriendshipRequests.Contains(rejecter) && rejecter.ReceivedFriendshipRequests.Contains(sender))
        || (sender.Friends.Contains(rejecter) && !rejecter.Friends.Contains(sender))
        || (!sender.Friends.Contains(rejecter) && rejecter.Friends.Contains(sender)))
        {
            _logger.LogError("There is a problem with this friendship. SenderId: {senderId}, RejecterId: {rejecterId}", sender.Id, rejecter.Id);
            return;
        }
        if (!rejecter.ReceivedFriendshipRequests.Contains(sender) || !sender.SentFriendshipRequests.Contains(rejecter))
        {
            _logger.LogWarning("There is no such friendship request. SenderId: {senderId}, RejecterId: {rejecterId}", sender.Id, rejecter.Id);
            return;
        }
        if (sender.Friends.Contains(rejecter) && rejecter.Friends.Contains(sender))
        {
            _logger.LogWarning("Friendship already continues. SenderId: {senderId}, RejecterId: {rejecterId}", sender.Id, rejecter.Id);
            return;
        }
        rejecter.ReceivedFriendshipRequests.Remove(sender);
        sender.SentFriendshipRequests.Remove(rejecter);
        dbContext.SaveChanges();
        _logger.LogInformation("Friendship request rejected. SenderId: {senderId}, RejecterId: {rejecterId}", sender.Id, rejecter.Id);
    }
}
