namespace Meet.RoomService.WebAPI.Entities;

public class Room
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public List<User> Users { get; set; } = null!;
    public List<Message>? Messages { get; set; }
    public List<Call>? Calls { get; set; }
}