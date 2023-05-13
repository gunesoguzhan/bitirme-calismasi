using Microsoft.EntityFrameworkCore;
using Meet.ProfileService.WebAPI.Entities;

namespace Meet.ProfileService.WebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>()
            .HasMany(x => x.Friends)
            .WithMany()
            .UsingEntity(
                "Friendships",
                l => l.HasOne(typeof(Profile))
                    .WithMany()
                    .HasForeignKey("Profile1Id"),
                r => r.HasOne(typeof(Profile))
                    .WithMany()
                    .HasForeignKey("Profile2Id")
                );

        modelBuilder.Entity<Profile>()
            .HasMany(e => e.SentFriendshipRequests)
            .WithMany(e => e.ReceivedFriendshipRequests)
            .UsingEntity(
                "FriendshipRequests",
                l => l.HasOne(typeof(Profile))
                    .WithMany()
                    .HasForeignKey("SenderId"),
                r => r.HasOne(typeof(Profile))
                    .WithMany()
                    .HasForeignKey("ReceiverId")
                );
    }
}