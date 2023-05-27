namespace Meet.IdentityService.WebAPI.Contracts;

public record UserRegistered(Guid id, string username, string firstName, string lastName);