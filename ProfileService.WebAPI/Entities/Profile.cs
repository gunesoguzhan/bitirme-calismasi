using System.ComponentModel.DataAnnotations;

namespace ProfileService.WebAPI.Entities;

public class Profile
{
    [Key]
    public Guid Id { get; init; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [MaxLength(50)]
    public required Guid UserId { get; set; }

    public List<Profile> Friends { get; set; } = new();

    public List<Profile> SentFriendshipRequests { get; set; } = new();

    public List<Profile> ReceivedFriendshipRequests { get; set; } = new();
}