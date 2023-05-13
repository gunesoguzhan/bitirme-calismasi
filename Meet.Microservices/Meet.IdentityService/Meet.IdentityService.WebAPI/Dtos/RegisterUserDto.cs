namespace Meet.IdentityService.WebAPI.Dtos;

public record RegisterUserDto(string firstName, string lastName, string username, string email, string password, string confirmPassword);