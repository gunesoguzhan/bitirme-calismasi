using Meet.IdentityService.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meet.IdentityService.WebAPI.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

}