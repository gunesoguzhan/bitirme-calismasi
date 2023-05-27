using Meet.RoomService.WebAPI.Dtos;
using Meet.RoomService.WebAPI.Entities;

namespace Meet.RoomService.WebAPI.Common;

public static class RoomExtension
{
    public static RoomDto AsDto(this Room entity)
        => new RoomDto(entity.Id, entity.Title);
}