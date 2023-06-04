namespace Meet.ProfileService.WebAPI.Contracts;

public record FriendshipRequestRejected(Guid rejecterId, Guid senderId);