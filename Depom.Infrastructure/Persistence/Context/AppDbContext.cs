using Microsoft.EntityFrameworkCore;
using DomainEntities = Depom.Domain.Entities;

namespace Depom.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DomainEntities.Category> Categories => Set<DomainEntities.Category>();
    public DbSet<DomainEntities.Product> Products => Set<DomainEntities.Product>();
    public DbSet<DomainEntities.Branch> Branches => Set<DomainEntities.Branch>();
    public DbSet<DomainEntities.AppUser> AppUsers => Set<DomainEntities.AppUser>();
    public DbSet<DomainEntities.StockItem> StockItems => Set<DomainEntities.StockItem>();
    public DbSet<DomainEntities.StockMovement> StockMovements => Set<DomainEntities.StockMovement>();
    public DbSet<DomainEntities.Transfer> Transfers => Set<DomainEntities.Transfer>();
    public DbSet<DomainEntities.TransferLog> TransferLogs => Set<DomainEntities.TransferLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DomainEntities.Product>(e =>
        {
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            e.HasIndex(x => x.SKU).IsUnique();
            e.HasOne(x => x.Category)
             .WithMany(x => x.Products)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DomainEntities.StockItem>(e =>
        {
            e.HasIndex(x => new { x.ProductId, x.BranchId }).IsUnique();
            e.HasOne(x => x.Product)
             .WithMany(x => x.StockItems)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Branch)
             .WithMany(x => x.StockItems)
             .HasForeignKey(x => x.BranchId)
             .OnDelete(DeleteBehavior.Restrict);
            e.Ignore(x => x.AvailableQuantity);
        });

        modelBuilder.Entity<DomainEntities.StockMovement>(e =>
        {
            e.HasOne(x => x.Product)
             .WithMany(x => x.StockMovements)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Branch)
             .WithMany(x => x.StockMovements)
             .HasForeignKey(x => x.BranchId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.CreatedByUser)
             .WithMany()
             .HasForeignKey(x => x.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Transfer)
             .WithMany(x => x.StockMovements)
             .HasForeignKey(x => x.TransferId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DomainEntities.Transfer>(e =>
        {
            e.HasOne(x => x.SourceBranch)
             .WithMany(x => x.OutgoingTransfers)
             .HasForeignKey(x => x.SourceBranchId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.TargetBranch)
             .WithMany(x => x.IncomingTransfers)
             .HasForeignKey(x => x.TargetBranchId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.CreatedByUser)
             .WithMany()
             .HasForeignKey(x => x.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Product)
             .WithMany()
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DomainEntities.TransferLog>(e =>
        {
            e.HasOne(x => x.Transfer)
             .WithMany(x => x.Logs)
             .HasForeignKey(x => x.TransferId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.ActingUser)
             .WithMany()
             .HasForeignKey(x => x.ActingUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DomainEntities.AppUser>(e =>
        {
            e.HasIndex(x => x.Username).IsUnique();
            e.HasOne(x => x.Branch)
             .WithMany(x => x.Users)
             .HasForeignKey(x => x.BranchId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
