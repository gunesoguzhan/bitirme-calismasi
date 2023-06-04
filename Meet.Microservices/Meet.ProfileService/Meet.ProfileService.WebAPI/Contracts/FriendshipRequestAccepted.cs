namespace Meet.ProfileService.WebAPI.Contracts;

public record FriendshipRequestAccepted(Guid accepterId, Guid senderId);