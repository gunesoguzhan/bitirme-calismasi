using System.Security.Claims;
using Meet.ChatService.WebAPI.Common;
using Meet.ChatService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.ChatService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> Get()
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
            var conversations = _dbContext.Conversations
                .Include(x => x.Messages)
                .Include(x => x.Users)
                .Select(x => x.AsDto());
            return Ok(conversations);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}