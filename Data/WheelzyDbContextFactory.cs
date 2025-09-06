using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Wheelzy.Data;


public class WheelzyDbContextFactory : IDesignTimeDbContextFactory<WheelzyDbContext>
{
    public WheelzyDbContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<WheelzyDbContext>()
        .UseSqlServer(
        "Server=localhost;Database=WheelzyDb;Trusted_Connection=True;TrustServerCertificate=True;")
        .Options;
        return new WheelzyDbContext(opts);
    }
}