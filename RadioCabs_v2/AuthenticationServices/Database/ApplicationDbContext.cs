using AuthenticationServices.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServices.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriverInfo> DriverInfos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserInfo)
            .WithOne(ui => ui.User)
            .HasForeignKey<UserInfo>(ui => ui.UserId);
        
        modelBuilder.Entity<Driver>()
            .HasOne(d => d.DriverInfo)
            .WithOne(di => di.Driver)
            .HasForeignKey<DriverInfo>(di => di.DriverId);
    }
}