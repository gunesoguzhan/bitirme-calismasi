using Meet.RoomService.WebAPI.Dtos;

namespace Meet.Contracts;

public record MessageReceived(string messageText, DateTime date, Guid userId, Guid roomId);