using Microsoft.EntityFrameworkCore;
using WheelzyProject.Data.Models;

namespace Wheelzy.Data;

public class WheelzyDbContext : DbContext
{
    public WheelzyDbContext(DbContextOptions<WheelzyDbContext> options) : base(options) { }

    public DbSet<CarMake> CarMakes => Set<CarMake>();
    public DbSet<CarModel> CarModels => Set<CarModel>();
    public DbSet<CarSubmodel> CarSubmodels => Set<CarSubmodel>();
    public DbSet<ZipCode> ZipCodes => Set<ZipCode>();
    public DbSet<Buyer> Buyers => Set<Buyer>();
    public DbSet<BuyerZipQuote> BuyerZipQuotes => Set<BuyerZipQuote>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CaseQuote> CaseQuotes => Set<CaseQuote>();
    public DbSet<CaseStatus> CaseStatuses => Set<CaseStatus>();
    public DbSet<CaseStatusHistory> CaseStatusHistories => Set<CaseStatusHistory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<CarMake>(e =>
        {
            e.HasKey(x => x.MakeId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<CarModel>(e =>
        {
            e.HasKey(x => x.ModelId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);

            e.HasOne(x => x.Make)
             .WithMany(m => m.Models)
             .HasForeignKey(x => x.MakeId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => new { x.MakeId, x.Name }).IsUnique();
        });

        b.Entity<CarSubmodel>(e =>
        {
            e.HasKey(x => x.SubmodelId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);

            e.HasOne(x => x.Model)
             .WithMany(m => m.Submodels)
             .HasForeignKey(x => x.ModelId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => new { x.ModelId, x.Name }).IsUnique();
        });

        b.Entity<ZipCode>(e =>
        {
            e.HasKey(x => x.Code);
            e.Property(x => x.Code).HasColumnName("ZipCode").HasColumnType("char(5)");
        });

        b.Entity<Buyer>(e =>
        {
            e.HasKey(x => x.BuyerId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<BuyerZipQuote>(e =>
        {
            e.HasKey(x => new { x.BuyerId, x.ZipCode });
            e.Property(x => x.Amount).HasColumnType("decimal(10,2)");
            e.Property(x => x.IsActive).HasDefaultValue(true);

            e.HasOne(x => x.Buyer)
             .WithMany()
             .HasForeignKey(x => x.BuyerId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Zip)
             .WithMany()
             .HasForeignKey(x => x.ZipCode)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => x.ZipCode).HasDatabaseName("IX_BuyerZipQuote_Zip");
        });

        b.Entity<Customer>(e =>
        {
            e.HasKey(x => x.CustomerId);
            e.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            e.Property(x => x.Balance).HasColumnType("decimal(18,2)");
        });

        b.Entity<Car>(e =>
        {
            e.HasKey(x => x.CarId);
            e.Property(x => x.Year).IsRequired();

            e.HasOne(x => x.Make)
             .WithMany()
             .HasForeignKey(x => x.MakeId)
             .OnDelete(DeleteBehavior.NoAction); 

            e.HasOne(x => x.Model)
             .WithMany()
             .HasForeignKey(x => x.ModelId)
             .OnDelete(DeleteBehavior.NoAction); 

            e.HasOne(x => x.Submodel)
             .WithMany()
             .HasForeignKey(x => x.SubmodelId)
             .OnDelete(DeleteBehavior.SetNull);  
        });

        b.Entity<Case>(e =>
        {
            e.ToTable("Case");
            e.HasKey(x => x.CaseId);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

            e.HasOne(x => x.Customer)
             .WithMany()
             .HasForeignKey(x => x.CustomerId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Car)
             .WithMany()
             .HasForeignKey(x => x.CarId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Zip)
             .WithMany()
             .HasForeignKey(x => x.ZipCode)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => x.ZipCode).HasDatabaseName("IX_Case_Zip");
            e.HasIndex(x => x.CustomerId).HasDatabaseName("IX_Case_Customer");
        });

        b.Entity<CaseQuote>(e =>
        {
            e.HasKey(x => x.CaseQuoteId);
            e.Property(x => x.Amount).HasColumnType("decimal(10,2)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.Property(x => x.IsCurrent).HasDefaultValue(false);

            e.HasOne(x => x.Case)
             .WithMany()
             .HasForeignKey(x => x.CaseId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Buyer)
             .WithMany()
             .HasForeignKey(x => x.BuyerId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => new { x.CaseId, x.BuyerId }).IsUnique();

            e.HasIndex(x => x.CaseId)
             .IsUnique()
             .HasFilter("[IsCurrent] = 1")
             .HasDatabaseName("UX_CaseQuote_CurrentPerCase");
        });

        b.Entity<CaseStatus>(e =>
        {
            e.HasKey(x => x.StatusId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.RequiresStatusDate).HasDefaultValue(false);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<CaseStatusHistory>(e =>
        {
            e.HasKey(x => x.CaseStatusHistoryId);
            e.Property(x => x.ChangedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.Property(x => x.IsCurrent).HasDefaultValue(false);

            e.HasOne(x => x.Case)
             .WithMany()
             .HasForeignKey(x => x.CaseId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Status)
             .WithMany()
             .HasForeignKey(x => x.StatusId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasIndex(x => x.CaseId)
             .IsUnique()
             .HasFilter("[IsCurrent] = 1")
             .HasDatabaseName("UX_CaseStatusHistory_CurrentPerCase");

        });

        b.Entity<OrderStatus>(e =>
        {
            e.HasKey(x => x.StatusId);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Order>(e =>
        {
            e.HasKey(x => x.OrderId);
            e.Property(x => x.Total).HasColumnType("decimal(18,2)");
            e.Property(x => x.IsActive).HasDefaultValue(true);

            e.HasOne(x => x.Customer)
             .WithMany()
             .HasForeignKey(x => x.CustomerId)
             .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Status)
             .WithMany()
             .HasForeignKey(x => x.StatusId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        foreach (var fk in b.Model.GetEntityTypes()
                 .SelectMany(t => t.GetForeignKeys())
                 .Where(fk => !fk.IsOwnership))
        {
            if (fk.DeleteBehavior != DeleteBehavior.SetNull)
                fk.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }
}
