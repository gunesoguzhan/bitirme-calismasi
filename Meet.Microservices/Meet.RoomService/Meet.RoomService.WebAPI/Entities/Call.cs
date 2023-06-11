using System.ComponentModel.DataAnnotations;

namespace Meet.RoomService.WebAPI.Entities;

public class Call
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public User Caller { get; set; } = null!;
    public Room Room { get; set; } = null!;
}