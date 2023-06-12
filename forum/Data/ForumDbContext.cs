using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; } = default!;

    public DbSet<Post> Post { get; set; } = default!;

    public DbSet<Message> Message { get; set; } = default!;

    public DbSet<Comment> Comment { get; set; } = default!;

    public DbSet<Tag> Tag { get; set; } = default!;

}