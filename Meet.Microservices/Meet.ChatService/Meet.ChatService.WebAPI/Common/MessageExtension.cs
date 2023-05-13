using Meet.ChatService.WebAPI.Dtos;
using Meet.ChatService.WebAPI.Entities;

namespace Meet.ChatService.WebAPI.Common;

public static class MessageExtension
{
    public static MessageDto AsDto(this Message message)
        => new MessageDto(message.Id, message.MessageText, message.Date, message.Sender.AsDto());
}