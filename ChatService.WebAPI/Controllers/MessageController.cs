using System.Security.Claims;
using ChatService.WebAPI.Common;
using ChatService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ApplicationDbContext dbContext, ILogger<MessageController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetMessagesFromConversationId([FromQuery] Guid conversationId)
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
        {
            _logger.LogWarning("There is a problem with JWT.");
            return BadRequest();
        }
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var conversation = _dbContext.Conversations
                .Include(x => x.Users)
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == conversationId && x.Users.Contains(user));
            var messages = conversation?.Messages.Select(x => x.AsDto());
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
}