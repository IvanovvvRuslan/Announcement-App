using Announcement_Web_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Announcement_Web_API.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    
    public DbSet<Announcement> Announcements { get; set; }
}