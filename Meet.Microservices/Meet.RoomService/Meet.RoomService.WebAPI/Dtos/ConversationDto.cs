namespace Meet.RoomService.WebAPI.Dtos;

public record ConversationDto(Guid id, MessageDto? lastMessage, RoomDto room);