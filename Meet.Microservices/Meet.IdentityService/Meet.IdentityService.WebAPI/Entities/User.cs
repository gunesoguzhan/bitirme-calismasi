using System.ComponentModel.DataAnnotations;

namespace Meet.IdentityService.WebAPI.Entities;

public class User
{
    [Key]
    public Guid Id { get; init; }

    [MaxLength(30)]
    public string Username { get; set; } = null!;

    [EmailAddress]
    [MaxLength(360)]
    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;
}