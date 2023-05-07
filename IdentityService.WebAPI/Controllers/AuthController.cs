using FluentValidation;
using IdentityService.WebAPI.Data;
using IdentityService.WebAPI.Dtos;
using IdentityService.WebAPI.Features.Hashing;
using IdentityService.WebAPI.Features.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IValidator<LoginUserDto> _loginUserDtoValidator;
    private readonly IValidator<RegisterUserDto> _registerUserDtoValidator;
    private readonly JwtTokenHandler _jwtTokenHandler;
    private readonly ApplicationDbContext _dbContext;
    private readonly PasswordHasher _passwordHasher;

    public AuthController
        (
            IValidator<LoginUserDto> loginUserDtoValidator,
            IValidator<RegisterUserDto> registerUserDtoValidator,
            JwtTokenHandler jwtTokenHandler,
            ApplicationDbContext dbContext,
            PasswordHasher passwordHasher)
    {
        _loginUserDtoValidator = loginUserDtoValidator;
        _registerUserDtoValidator = registerUserDtoValidator;
        _jwtTokenHandler = jwtTokenHandler;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        //Validate model. If model is not valid return 400.
        var validationResult = await _loginUserDtoValidator.ValidateAsync(loginUserDto);
        if (!validationResult.IsValid)
            return BadRequest();

        // Return token. If there is an exception return 500.
        try
        {
            // Get user.
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Username == loginUserDto.usernameOrEmail || x.Email == loginUserDto.usernameOrEmail);

            // Verify password. If there is no user or passwords do not match return 404.
            if (user == null || !_passwordHasher.VerifyPassword(user.HashedPassword, loginUserDto.password))
                return NotFound();
            return Content(_jwtTokenHandler.GenerateToken(user, TimeSpan.FromMinutes(20)));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        //Validate model. If model is not valid return 400;
        var validationResult = await _registerUserDtoValidator.ValidateAsync(registerUserDto);
        if (!validationResult.IsValid)
            return BadRequest();

        // Check if user is already created. If it is already created return 400.
        var user = _dbContext.Users.FirstOrDefault(x => x.Username == registerUserDto.username || x.Email == registerUserDto.email);
        if (user != null)
            return BadRequest();

        // Hash password. If there is an exception return 500.
        string hashedPassword;
        try
        {
            hashedPassword = _passwordHasher.HashPassword(registerUserDto.password);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // Create user.
        user = new()
        {
            Id = Guid.NewGuid(),
            Username = registerUserDto.username,
            Email = registerUserDto.email,
            HashedPassword = hashedPassword
        };

        // Insert user to the database. If there is an exception return 500.
        try
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}