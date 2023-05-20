using System.ComponentModel.DataAnnotations;

namespace Meet.ChatService.WebAPI.Entities;

public class Conversation
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public List<Message> Messages { get; } = null!;

    public List<User> Users { get; } = null!;
}