namespace Tracker.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<RepairRequest> RepairRequests { get; set; }

    // Конструктор по умолчанию
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
