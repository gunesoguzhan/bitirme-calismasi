namespace Meet.RoomService.WebAPI.Entities;

public class Room
{
    public Guid Id { get; set; }
    public List<Guid> Users { get; set; } = null!;
}