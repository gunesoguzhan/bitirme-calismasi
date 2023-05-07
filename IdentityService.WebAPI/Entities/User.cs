using System.ComponentModel.DataAnnotations;

namespace IdentityService.WebAPI.Entities;

public class User
{
    [Key]
    public Guid Id { get; init; }

    [MaxLength(30)]
    public required string Username { get; set; }

    [EmailAddress]
    [MaxLength(360)]
    public required string Email { get; set; }

    public required string HashedPassword { get; set; }
}