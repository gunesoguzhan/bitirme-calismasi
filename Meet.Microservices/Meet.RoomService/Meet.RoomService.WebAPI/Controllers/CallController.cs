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
public class CallController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CallController> _logger;

    public CallController(ApplicationDbContext dbContext, ILogger<CallController> logger)
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
            var calls = _dbContext.Calls.Include(x => x.Room).Where(x => x.Room.Users.Contains(user));
            if (calls == null || calls.Count() == 0)
            {
                _logger.LogInformation("No call found. UserId: {userId}", userId);
                return NotFound();
            }
            _logger.LogInformation("Calls listed. UserId: {userId}", userId);
            return Ok(calls.Select(x => x.AsDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught {ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{callId}")]
    public async Task<IActionResult> GetByIdAsync(Guid callId)
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
            var call = await _dbContext.Calls.Include(x => x.Room).FirstOrDefaultAsync(x => x.Id == callId);
            if (call == null)
            {
                _logger.LogInformation("No call found. UserId: {userId} CallId: {callId}", userId, callId);
                return NotFound();
            }
            _logger.LogInformation("Call found. UserId: {userId} CallId: {callId}", userId, callId);
            return Ok(call.AsDto());
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught {ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}