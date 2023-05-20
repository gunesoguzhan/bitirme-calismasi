using System.Security.Claims;
using Meet.ChatService.WebAPI.Common;
using Meet.ChatService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.ChatService.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class ConversationController : ControllerBase
{

    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ConversationController> _logger;

    public ConversationController(ApplicationDbContext dbContext, ILogger<ConversationController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetConversations()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        try
        {
            var userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return NotFound();
            var conversations = _dbContext.Conversations
                .Where(x => x.Users.Contains(user))
                .Include(x => x.Messages);
            return Ok(conversations.Select(x => x.AsDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}