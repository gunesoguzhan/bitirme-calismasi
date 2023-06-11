namespace Meet.RoomService.WebAPI.Contracts;

public record CallReceived(Guid callerId, DateTime date, Guid roomId);