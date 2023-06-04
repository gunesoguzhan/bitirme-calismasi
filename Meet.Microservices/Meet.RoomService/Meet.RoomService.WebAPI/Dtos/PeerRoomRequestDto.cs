namespace Meet.RoomService.WebAPI.Dtos;

public record PeerRoomRequestDto(UserDto creator, UserDto friend);