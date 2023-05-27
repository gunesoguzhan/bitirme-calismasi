namespace Meet.RoomService.WebAPI.Dtos;

public record MessageDto(Guid id, string messageText, UserDto sender, DateTime date);