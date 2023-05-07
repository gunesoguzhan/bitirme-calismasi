namespace IdentityService.WebAPI.Dtos;

public record RegisterUserDto(string username, string email, string password, string firstName, string lastName);