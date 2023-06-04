using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Meet.RoomService.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.RoomService.WebAPI.Controllers;

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
    public async Task<IActionResult> GetAsync()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        try
        {
            var userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found. UserId: {userId}", userId);
                return NotFound();
            }
            var rooms = _dbContext.Rooms?
                .Where(x => x.Users.Contains(user) && x.Messages.Count > 0)
                .Include(x => x.Messages)
                .ThenInclude(x => x.User);
            if (rooms == null || rooms.Count() == 0)
            {
                _logger.LogInformation("No room found. UserId: {userId}", userId);
                return NotFound();
            }
            _logger.LogInformation("Conversations listed. UserId: {userId}", userId);
            return Ok(rooms.Select(x => x.AsConversationDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught {ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}