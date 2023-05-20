using System.ComponentModel.DataAnnotations;

namespace Meet.ChatService.WebAPI.Entities;

public class Message
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(1000)]
    public string MessageText { get; set; } = null!;

    public DateTime Date { get; set; }

    public User Sender { get; set; } = null!;

    public Conversation Conversation { get; set; } = null!;
}