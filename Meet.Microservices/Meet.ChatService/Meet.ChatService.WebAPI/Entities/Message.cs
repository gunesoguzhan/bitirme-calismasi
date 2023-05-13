namespace Meet.ChatService.WebAPI.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string MessageText { get; set; } = null!;
    public DateTime Date { get; set; }
    public User Sender { get; set; } = null!;
    public Conversation Conversation { get; set; } = null!;
}