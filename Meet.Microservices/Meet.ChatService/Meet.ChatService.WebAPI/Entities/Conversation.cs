using System.ComponentModel.DataAnnotations;

namespace Meet.ChatService.WebAPI.Entities;

public class Conversation
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public List<Message> Messages { get; } = new();

    public List<User> Users { get; } = new();
}