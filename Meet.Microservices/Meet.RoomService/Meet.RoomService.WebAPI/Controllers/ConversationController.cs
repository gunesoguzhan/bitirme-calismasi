using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Meet.RoomService.WebAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.RoomService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
            if (user == null) return NotFound();
            var rooms = _dbContext.Rooms
                .Where(x => x.Users.Contains(user))
                .Include(x => x.Messages);
            return Ok(rooms.Select(x => x.AsConversationDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}