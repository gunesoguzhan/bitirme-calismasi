using System.Text.Json;
using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Contracts;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Meet.ProfileService.WebAPI.Consumers;

public class FriendshipRemovedConsumer : RabbitMQConsumerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;

    public FriendshipRemovedConsumer(IConfiguration configuration, ILogger<FriendshipRemovedConsumer> logger, IServiceScopeFactory scopeFactory)
        : base("Meet-FriendshipRemoved", configuration, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override void HandleMessage(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var @object = JsonSerializer.Deserialize<FriendshipRemoved>(message);
        if (@object == null)
        {
            _logger.LogError("Parse error.");
            return;
        }
        var remover = dbContext.Profiles
                    .Include(x => x.ReceivedFriendshipRequests)
                    .Include(x => x.SentFriendshipRequests)
                    .Include(x => x.Friends)
                    .FirstOrDefault(x => x.Id == @object.removerId);
        var other = dbContext.Profiles
                    .Include(x => x.ReceivedFriendshipRequests)
                    .Include(x => x.SentFriendshipRequests)
                    .Include(x => x.Friends)
            .FirstOrDefault(x => x.Id == @object.friendId);
        if (remover == null || other == null)
        {
            _logger.LogError("Remover or other is not found.");
            return;
        }
        if ((other.SentFriendshipRequests.Contains(remover) && !remover.ReceivedFriendshipRequests.Contains(other))
        || (!other.SentFriendshipRequests.Contains(remover) && remover.ReceivedFriendshipRequests.Contains(other))
        || (other.Friends.Contains(remover) && !remover.Friends.Contains(other))
        || (!other.Friends.Contains(remover) && remover.Friends.Contains(other)))
        {
            _logger.LogError("There is a problem with this friendship. OtherId: {otherId}, RemoverId: {removerId}", other.Id, remover.Id);
            return;
        }
        if (!remover.Friends.Contains(other) || !other.Friends.Contains(remover))
        {
            _logger.LogWarning("There is no such friendship request. OtherId: {otherId}, RemoverId: {removerId}", other.Id, remover.Id);
            return;
        }
        remover.Friends.Remove(other);
        other.Friends.Remove(remover);
        dbContext.SaveChanges();
        _logger.LogInformation("Friendship removed. OtherId: {otherId}, RemoverId: {removerId}", other.Id, remover.Id);
    }
}
