using Meet.ChatService.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace Meet.ChatService.WebAPI.DataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}