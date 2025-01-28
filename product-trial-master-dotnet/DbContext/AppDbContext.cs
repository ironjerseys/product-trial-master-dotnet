using Microsoft.EntityFrameworkCore;
using product_trial_master_dotnet.Models;

namespace product_trial_master_dotnet.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}
