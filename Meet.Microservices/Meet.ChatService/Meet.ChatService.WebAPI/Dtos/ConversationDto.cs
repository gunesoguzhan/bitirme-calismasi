namespace Meet.ChatService.WebAPI.Dtos;

public record ConversationDto(Guid id, string title, MessageDto lastMessage);