using LogisticsApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Data;

public class LogisticsDbContext : IdentityDbContext
{
    public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : base(options)
    {
    }

    #region DbSet

    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<OrderTracking> OrderTrackings { get; set; }
    public DbSet<StatusOrder> StatusOrders { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<UserOTP> UserOTPs { get; set; }
    public DbSet<OrderPackage> OrderPackages { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
        builder.Entity<Driver>()
            .HasOne(d => d.ApplicationUser)
            .WithOne(u => u.Driver)
            .HasForeignKey<Driver>(d => d.UserId);

        builder.Entity<OrderPackage>().HasKey(op => new { op.OrderId, op.PackageId });
        builder.Entity<OrderPackage>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderPackages)
            .HasForeignKey(op => op.OrderId);
        builder.Entity<OrderPackage>()
            .HasOne(op => op.Package)
            .WithMany(p => p.OrderPackages)
            .HasForeignKey(op => op.PackageId);
    }
}