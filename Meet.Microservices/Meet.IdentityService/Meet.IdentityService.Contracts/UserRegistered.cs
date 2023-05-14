namespace Meet.IdentityService.Contracts;

public record UserRegistered(Guid Id, string username, string firstName, string lastName);