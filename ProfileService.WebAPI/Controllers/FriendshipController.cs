using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileService.WebAPI.Common;
using ProfileService.WebAPI.Data;
using ProfileService.WebAPI.Dtos;

namespace ProfileService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FriendshipController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IValidator<SendFriendshipRequestDto> _sendFriendshipRequestDtoValidator;
    private readonly IValidator<AcceptFriendshipRequestDto> _acceptFriendshipRequestDtoValidator;
    private readonly IValidator<RejectFriendshipRequestDto> _rejectFriendshipRequestDtoValidator;
    private readonly IValidator<CancelFriendshipRequestDto> _cancelFriendshipRequestDtoValidator;
    private readonly IValidator<RemoveFriendshipDto> _removeFriendshipDtoValidator;

    public FriendshipController
        (
            ApplicationDbContext dbContext,
            IValidator<SendFriendshipRequestDto> sendFriendshipRequestDtoValidator,
            IValidator<AcceptFriendshipRequestDto> acceptFriendshipRequestDtoValidator,
            IValidator<RejectFriendshipRequestDto> rejectFriendshipRequestDtoValidator,
            IValidator<CancelFriendshipRequestDto> cancelFriendshipRequestDtoValidator,
            IValidator<RemoveFriendshipDto> removeFriendshipDtoValidator
        )
    {
        _dbContext = dbContext;
        _sendFriendshipRequestDtoValidator = sendFriendshipRequestDtoValidator;
        _acceptFriendshipRequestDtoValidator = acceptFriendshipRequestDtoValidator;
        _rejectFriendshipRequestDtoValidator = rejectFriendshipRequestDtoValidator;
        _cancelFriendshipRequestDtoValidator = cancelFriendshipRequestDtoValidator;
        _removeFriendshipDtoValidator = removeFriendshipDtoValidator;
    }

    //Send friendship request
    [HttpPost("SendFriendshipRequest")]
    public async Task<IActionResult> SendFriendshipRequest(SendFriendshipRequestDto sendFriendshipRequestDto)
    {
        var validationResults = await _sendFriendshipRequestDtoValidator
            .ValidateAsync(sendFriendshipRequestDto);
        if (!validationResults.IsValid)
            return BadRequest();
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var sender = await _dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var receiver = await _dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == sendFriendshipRequestDto.friendUserId);
        if (sender == null || receiver == null)
            return NotFound();
        if (sender.SentFriendshipRequests.Contains(receiver))
            return BadRequest();
        if (receiver.SentFriendshipRequests.Contains(sender))
        {
            receiver.SentFriendshipRequests.Remove(sender);
            sender.Friends.Add(receiver);
            receiver.Friends.Add(sender);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        sender.SentFriendshipRequests.Add(receiver);
        receiver.ReceivedFriendshipRequests.Add(sender);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    //Accept friendship request
    [HttpPost("AcceptFriendshipRequest")]
    public async Task<IActionResult> AcceptFriendshipRequest(AcceptFriendshipRequestDto acceptFriendshipRequestDto)
    {
        var validationResults = await _acceptFriendshipRequestDtoValidator
            .ValidateAsync(acceptFriendshipRequestDto);
        if (!validationResults.IsValid)
            return BadRequest();
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var accepter = await _dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var sender = await _dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == acceptFriendshipRequestDto.friendUserId);
        if (accepter == null || sender == null)
            return NotFound();
        if (!accepter.ReceivedFriendshipRequests.Contains(sender))
            return BadRequest();
        accepter.ReceivedFriendshipRequests.Remove(sender);
        sender.SentFriendshipRequests.Remove(accepter);
        accepter.Friends.Add(sender);
        sender.Friends.Add(accepter);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    //Reject friendship request
    [HttpPost("RejectFriendshipRequest")]
    public async Task<IActionResult> RejectFriendshipRequest(RejectFriendshipRequestDto rejectFriendshipRequestDto)
    {
        var validationResults = await _rejectFriendshipRequestDtoValidator
            .ValidateAsync(rejectFriendshipRequestDto);
        if (!validationResults.IsValid)
            return BadRequest();
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var rejecter = await _dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var sender = await _dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .FirstOrDefaultAsync(x => x.UserId == rejectFriendshipRequestDto.friendUserId);
        if (rejecter == null || sender == null)
            return NotFound();
        if (!rejecter.ReceivedFriendshipRequests.Contains(sender))
            return BadRequest();
        rejecter.ReceivedFriendshipRequests.Remove(sender);
        sender.SentFriendshipRequests.Remove(rejecter);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    //Cancel friendship request
    [HttpPost("CancelFriendshipRequest")]
    public async Task<IActionResult> CancelFriendshipRequest(CancelFriendshipRequestDto cancelFriendshipRequestDto)
    {
        var validationResults = await _cancelFriendshipRequestDtoValidator
            .ValidateAsync(cancelFriendshipRequestDto);
        if (!validationResults.IsValid)
            return BadRequest();
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var canceller = await _dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var sender = await _dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .FirstOrDefaultAsync(x => x.UserId == cancelFriendshipRequestDto.friendUserId);
        if (canceller == null || sender == null)
            return NotFound();
        if (!canceller.SentFriendshipRequests.Contains(sender))
            return BadRequest();
        canceller.SentFriendshipRequests.Remove(sender);
        sender.ReceivedFriendshipRequests.Remove(canceller);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    //Remove friendship
    [HttpPost("RemoveFriendship")]
    public async Task<IActionResult> RemoveFriendship(RemoveFriendshipDto removeFriendshipDto)
    {
        var validationResults = await _removeFriendshipDtoValidator
            .ValidateAsync(removeFriendshipDto);
        if (!validationResults.IsValid)
            return BadRequest();
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var remover = await _dbContext.Profiles
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var other = await _dbContext.Profiles
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == removeFriendshipDto.friendUserId);
        if (remover == null || other == null)
            return NotFound();
        if (!remover.Friends.Contains(other) || !other.Friends.Contains(remover))
            return BadRequest();
        remover.Friends.Remove(other);
        other.Friends.Remove(remover);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("GetFriends")]
    public async Task<IActionResult> GetFriends()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var user = await _dbContext.Profiles
            .Include(x => x.Friends)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return NotFound();
        return Ok(user.Friends.Select(x => x.AsDto()));
    }

    [HttpGet("GetSentFriendshipRequests")]
    public async Task<IActionResult> GetSentFriendshipRequestsByUsername()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var user = await _dbContext.Profiles
            .Include(x => x.SentFriendshipRequests)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return NotFound();
        return Ok(user.SentFriendshipRequests.Select(x => x.AsDto()));
    }

    [HttpGet("GetReceivedFriendshipRequests")]
    public async Task<IActionResult> GetReceivedFriendshipRequestsByUsername()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null)
            return NotFound();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        var user = await _dbContext.Profiles
            .Include(x => x.ReceivedFriendshipRequests)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return NotFound();
        return Ok(user.ReceivedFriendshipRequests.Select(x => x.AsDto()));
    }
    //Block user
    //Unblock user
    //Get blocked users
}