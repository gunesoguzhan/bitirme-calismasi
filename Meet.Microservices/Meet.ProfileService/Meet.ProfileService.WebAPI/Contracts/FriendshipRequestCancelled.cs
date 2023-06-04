namespace Meet.ProfileService.WebAPI.Contracts;

public record FriendshipRequestCancelled(Guid cancellerId, Guid friendId);