using System.ComponentModel.DataAnnotations;

namespace Meet.RoomService.WebAPI.Entities;

public class User
{
    [Key]
    public Guid Id { get; init; }

    [StringLength(30)]
    public string Username { get; set; } = null!;

    [StringLength(30)]
    public string FirstName { get; set; } = null!;

    [StringLength(30)]
    public string LastName { get; set; } = null!;

    public List<Room> Rooms { get; set; } = null!;
}