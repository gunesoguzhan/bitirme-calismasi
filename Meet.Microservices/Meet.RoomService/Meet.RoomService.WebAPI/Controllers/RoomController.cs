using System.Security.Claims;
using Meet.RoomService.WebAPI.Common;
using Meet.RoomService.WebAPI.DataAccess;
using Meet.RoomService.WebAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.RoomService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RoomController> _logger;

    public RoomController(ApplicationDbContext dbContext, ILogger<RoomController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
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
            var rooms = _dbContext.Rooms.Where(x => x.Users.Contains(user));
            if (rooms == null || rooms.Count() == 0)
            {
                _logger.LogWarning("No room found. UserId: {userId}", userId);
                return NotFound();
            }
            _logger.LogInformation("Rooms listed. UserId: {userId}", userId);
            return Ok(rooms.Select(x => x.AsDto()));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("FindOrCreatePeerRoom")]
    public async Task<IActionResult> FindOrCreatePeerRoom([FromBody] PeerRoomRequestDto peerRoomRequestDto)
    {
        var userIdString = User.FindFirstValue("userId");
        if (userIdString == null)
            return Unauthorized();
        try
        {
            var userId = Guid.Parse(userIdString);
            var user = await _dbContext.Users.Include(x => x.Rooms).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                _logger.LogInformation("User not found. UserId: {userId}", userId);
                user = new()
                {
                    Id = peerRoomRequestDto.creator.id,
                    FirstName = peerRoomRequestDto.creator.firstName,
                    LastName = peerRoomRequestDto.creator.lastName
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("User created. UserId: {userId}", userId);
            }
            var friend = await _dbContext.Users.Include(x => x.Rooms).FirstOrDefaultAsync(x => x.Id == peerRoomRequestDto.friend.id);
            if (friend == null)
            {
                _logger.LogInformation("Friend not found. UserId: {userId}", peerRoomRequestDto.friend.id);
                friend = new()
                {
                    Id = peerRoomRequestDto.friend.id,
                    FirstName = peerRoomRequestDto.friend.firstName,
                    LastName = peerRoomRequestDto.friend.lastName
                };
                await _dbContext.Users.AddAsync(friend);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Friend created. UserId: {userId}", peerRoomRequestDto.friend.id);
            }
            var room = _dbContext.Rooms.FirstOrDefault(x =>
                x.Users.Count == 2
                && x.Users.Contains(user)
                && x.Users.Contains(friend));
            bool created = false;
            if (room == null)
            {
                room = new()
                {
                    Title = "",
                    Users = new() { user, friend }
                };
                await _dbContext.Rooms.AddAsync(room);
                await _dbContext.SaveChangesAsync();
                created = true;
                _logger.LogInformation("Room created. RoomId: {roomId}, userId: {userId}", room.Id, userId);
            }
            if (user.Rooms == null)
                user.Rooms = new();
            if (friend.Rooms == null)
                friend.Rooms = new();
            var userRoom = user.Rooms.FirstOrDefault(x => x.Id == room.Id);
            if (userRoom == null)
                user.Rooms.Add(room);
            var friendRoom = friend.Rooms.FirstOrDefault(x => x.Id == room.Id);
            if (friendRoom == null)
                friend.Rooms.Add(room);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Room listed. RoomId: {roomId}, userId: {userId}", room.Id, userId);
            return Ok(room.AsPeerRoomDto(created));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}