using Meet.RoomService.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace Meet.RoomService.WebAPI.DataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Call> Calls { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}