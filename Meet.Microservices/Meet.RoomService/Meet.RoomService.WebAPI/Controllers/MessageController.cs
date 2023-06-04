using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.RoomService.WebAPI.Controllers;

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
    public async Task<IActionResult> GetMessagesByRoomIdAsync([FromQuery] Guid roomId)
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found. UserId: {userId}", userId);
                return NotFound();
            }
            var room = _dbContext.Rooms
                .FirstOrDefault(x => x.Id == roomId && x.Users.Contains(user));
            if (room == null)
            {
                _logger.LogInformation("No room found. UserId: {userId}", userId);
                return NotFound();
            }
            var messages = _dbContext.Messages
                .Where(x => x.Room.Id == room.Id)
                .Include(x => x.User)
                .Include(x => x.Room);
            if (messages == null || messages.Count() == 0)
            {
                _logger.LogInformation("No message found. UserId: {userId}, RoomId: {roomId}", userId, room.Id);
                return NotFound();
            }
            _logger.LogInformation("Messages listed. UserId: {userId}", userId);
            return Ok(messages.Select(x => x.AsDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}