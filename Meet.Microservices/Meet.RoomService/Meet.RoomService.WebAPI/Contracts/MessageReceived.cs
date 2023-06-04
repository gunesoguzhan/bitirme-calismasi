namespace Meet.RoomService.WebAPI.Contracts;

public record MessageReceived(string messageText, DateTime date, Guid userId, Guid roomId);