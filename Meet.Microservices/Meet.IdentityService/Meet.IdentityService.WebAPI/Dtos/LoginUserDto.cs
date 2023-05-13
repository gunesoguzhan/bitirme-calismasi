namespace Meet.IdentityService.WebAPI.Dtos;

public record LoginUserDto(string usernameOrEmail, string password);