using System.ComponentModel.DataAnnotations;

namespace Meet.IdentityService.WebAPI.Entities;

public class User
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(30)]
    public string Username { get; set; } = null!;

    [EmailAddress]
    [StringLength(360)]
    public string Email { get; set; } = null!;

    [StringLength(70)]
    public string HashedPassword { get; set; } = null!;
}