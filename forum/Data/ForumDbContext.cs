using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data;

public class ForumDbContext:DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> User { get; set; } = default!;
}