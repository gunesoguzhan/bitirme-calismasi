using Meet.ChatService.WebAPI.Dtos;
using Meet.ChatService.WebAPI.Entities;

namespace Meet.ChatService.WebAPI.Common;

public static class ConversationExtension
{
    public static ConversationDto AsDto(this Conversation conversation)
        => new ConversationDto(
            conversation.Id,
            conversation.Title,
            conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().AsDto());
}