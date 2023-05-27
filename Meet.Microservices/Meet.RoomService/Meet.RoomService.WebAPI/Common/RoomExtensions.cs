using Meet.RoomService.WebAPI.Dtos;
using Meet.RoomService.WebAPI.Entities;

namespace Meet.RoomService.WebAPI.Common;

public static class RoomExtensions
{
    public static RoomDto AsDto(this Room entity)
        => new RoomDto(entity.Id, entity.Title);

    public static ConversationDto AsConversationDto(this Room room)
        => new ConversationDto(room.Id, room.Messages.LastOrDefault().AsDto(), room.AsDto());
}