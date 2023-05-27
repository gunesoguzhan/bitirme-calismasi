using Meet.RoomService.WebAPI.Dtos;

namespace Meet.Contracts;

public record MessageReceived(string messageText, DateTime dateTime, Guid userId, Guid roomId);