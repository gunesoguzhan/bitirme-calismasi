namespace Meet.ProfileService.WebAPI.Contracts;

public record FriendshipRequestSent(Guid senderId, Guid receiverId);