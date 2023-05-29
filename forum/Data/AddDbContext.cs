using forum.Models;
using Microsoft.EntityFrameworkCore;

namespace forum.Data;

public class AddDbContext:DbContext
{
    protected AddDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Users> Users { get; set; }
}