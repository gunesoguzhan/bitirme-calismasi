using Meet.RoomService.WebAPI.Dtos;
using Meet.RoomService.WebAPI.Entities;

namespace Meet.RoomService.WebAPI.Common;

public static class CallExtensions
{
    public static CallDto AsDto(this Call call)
        => new CallDto(call.Id, call.Date, call.Caller.AsDto(), call.Room.AsDto());
}