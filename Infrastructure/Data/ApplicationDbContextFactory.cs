using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

/// <summary>
/// Design-time factory for creating ApplicationDbContext during migrations
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use a dummy connection string for migration script generation
        // This is only used for generating SQL scripts, not for actual runtime
        optionsBuilder.UseSqlServer("Server=localhost;Database=SavoryDb;Trusted_Connection=true;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}