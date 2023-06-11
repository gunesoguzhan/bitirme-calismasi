namespace Meet.RoomService.WebAPI.Dtos;

public record CallDto(Guid id, DateTime date, UserDto caller, RoomDto room);