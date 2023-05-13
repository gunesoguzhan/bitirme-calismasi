using System.ComponentModel.DataAnnotations;
namespace Meet.ChatService.WebAPI.Entities;

public class User
{
    public Guid Id { get; set; }

    [StringLength(30)]
    public string Username { get; set; } = null!;

    [StringLength(30)]
    public string FirstName { get; set; } = null!;

    [StringLength(30)]
    public string LastName { get; set; } = null!;

    public List<Conversation> Conversations { get; } = new();
}