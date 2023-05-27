using System.ComponentModel.DataAnnotations;

namespace Meet.RoomService.WebAPI.Entities;

public class Message
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(1000)]
    public string MessageText { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public User User { get; set; } = null!;

    public Room Room { get; set; } = null!;
}