using ChatService.WebAPI.Dtos;
using ChatService.WebAPI.Entities;

namespace ChatService.WebAPI.Common;

public static class ConversationExtension
{
    public static ConversationDto AsDto(this Conversation conversation)
        => new ConversationDto(
            conversation.Id,
            conversation.Title,
            conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().AsDto());
}