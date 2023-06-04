using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Meet.ProfileService.WebAPI.Common;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace Meet.ProfileService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FriendshipController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<FriendshipController> _logger;

    public FriendshipController(ApplicationDbContext dbContext, ILogger<FriendshipController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("GetFriends")]
    public async Task<IActionResult> GetFriends()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var user = await _dbContext.Profiles
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            _logger.LogInformation("User is not found. UserId: {userId}", userId);
            return NotFound();
        }
        _logger.LogInformation("Friends listed. UserId: {userId}", userId);
        return Ok(user.Friends.Select(x => x.AsDto()));
    }

    [HttpGet("GetSentFriendshipRequests")]
    public async Task<IActionResult> GetSentFriendshipRequestsByUsername()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var user = await _dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            _logger.LogInformation("User is not found. UserId: {userId}", userId);
            return NotFound();
        }
        _logger.LogInformation("Sent friendship requests listed. UserId: {userId}", userId);
        return Ok(user.SentFriendshipRequests.Select(x => x.AsDto()));
    }

    [HttpGet("GetReceivedFriendshipRequests")]
    public async Task<IActionResult> GetReceivedFriendshipRequestsByUsername()
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var user = await _dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            _logger.LogInformation("User is not found. UserId: {userId}", userId);
            return NotFound();
        }
        _logger.LogInformation("Received friendship requests listed. UserId: {userId}", userId);
        return Ok(user.ReceivedFriendshipRequests.Select(x => x.AsDto()));
    }

    [HttpGet("Search")]
    public IActionResult Search([FromQuery] string searchString)
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has been caught", ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        searchString = searchString.ToUpper();
        var users = _dbContext.Profiles.Where(x => (x.Id != userId)
            && (x.FirstName.ToUpper().Contains(searchString))
            || (x.LastName.ToUpper().Contains(searchString))
            || (searchString.Contains(x.FirstName.ToUpper() + " " + x.LastName.ToUpper())));
        if (users == null)
        {
            _logger.LogInformation("No user found. UserId: {userId}", userId);
            return NotFound();
        }
        _logger.LogInformation("Users listed. UserId: {userId}", userId);
        return Ok(users.Select(x => x.AsDto()));
    }
}