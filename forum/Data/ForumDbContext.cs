using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
        if (this.User.Count() == 0)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin");

            this.Add(new User { id = 1, name = "admin", lastName = "admin", email = "admin@admin.pl", passwordHash = passwordHash, token = Guid.NewGuid().ToString(), role = "admin" });
            this.SaveChanges();
        }
    }

    public DbSet<User> User { get; set; } = default!;

    public DbSet<Post> Post { get; set; } = default!;

    public DbSet<Message> Message { get; set; } = default!;

    public DbSet<Comment> Comment { get; set; } = default!;

    public DbSet<Tag> Tag { get; set; } = default!;

}