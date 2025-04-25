using Activities.Domain;
using Microsoft.EntityFrameworkCore;

namespace Activities.Persistence;
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Activity> Activities { get; set; }
}