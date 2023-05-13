namespace Meet.ChatService.WebAPI.Dtos;

public record MessageDto(Guid id, string messageText, DateTime date, UserDto sender);