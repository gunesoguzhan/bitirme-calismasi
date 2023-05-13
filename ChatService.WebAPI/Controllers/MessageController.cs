using ChatService.WebAPI.Common;
using ChatService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public MessageController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetMessagesFromConversationId([FromQuery] Guid conversationId)
    {
        var messages = _dbContext.Messages
            .Where(x => x.Conversation.Id == conversationId)
            .Include(x => x.Sender)
            .Select(x => x.AsDto());
        return Ok(messages);
    }
}