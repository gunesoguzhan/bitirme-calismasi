using ChatService.WebAPI.Dtos;
using ChatService.WebAPI.Entities;

namespace ChatService.WebAPI.Common;

public static class MessageExtension
{
    public static MessageDto AsDto(this Message message)
        => new MessageDto(message.Id, message.MessageText, message.Date, message.Sender.AsDto());
}