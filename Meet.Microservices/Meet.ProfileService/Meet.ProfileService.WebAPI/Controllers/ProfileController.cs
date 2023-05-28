using System.Security.Claims;
using Meet.ProfileService.WebAPI.Common;
using Meet.ProfileService.WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meet.ProfileService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ApplicationDbContext dbContext, ILogger<ProfileController> logger)
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
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(x => x.Id == userId);
            if (profile == null) return NotFound();
            return Ok(profile.AsDto());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}