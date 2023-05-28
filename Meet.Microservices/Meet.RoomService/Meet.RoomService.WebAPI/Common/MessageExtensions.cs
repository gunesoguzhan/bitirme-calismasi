using Meet.RoomService.WebAPI.Dtos;
using Meet.RoomService.WebAPI.Entities;

namespace Meet.RoomService.WebAPI.Common;

public static class MessageExtensions
{
    public static MessageDto AsDto(this Message message)
    => new(message.Id, message.MessageText, message.User.AsDto(), message.Date, message.Room.AsDto());
}