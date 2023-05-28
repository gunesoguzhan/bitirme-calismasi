using System.ComponentModel.DataAnnotations;

namespace Meet.ProfileService.WebAPI.Entities;

public class Profile
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(30)]
    public string FirstName { get; set; } = null!;

    [StringLength(30)]
    public string LastName { get; set; } = null!;

    public List<Profile> Friends { get; set; } = null!;

    public List<Profile> SentFriendshipRequests { get; set; } = null!;

    public List<Profile> ReceivedFriendshipRequests { get; set; } = null!;
}