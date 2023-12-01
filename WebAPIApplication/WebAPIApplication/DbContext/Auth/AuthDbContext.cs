using Microsoft.EntityFrameworkCore;
using WebAPIApplication.Model.Auth;

namespace WebAPIApplication.DbContext.Auth;

public class AuthDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasMany(e => e.UserPermissionsList)
            .WithOne(e => e.User)
            .HasForeignKey(key => key.UserId);
        builder.Entity<Permissions>()
            .HasMany(e => e.UserPermissionsList)
            .WithOne(e => e.Permissions)
            .HasForeignKey(key => key.PermissionsId);

        builder.Entity<UserPermissions>()
            .HasKey(e => new { e.UserId, e.PermissionsId });
        builder.Entity<UserPermissions>()
            .HasOne(e => e.User)
            .WithMany(e => e.UserPermissionsList)
            .HasForeignKey(key => key.UserId);
        builder.Entity<UserPermissions>()
            .HasOne(e => e.Permissions)
            .WithMany(e => e.UserPermissionsList)
            .HasForeignKey(key => key.PermissionsId);
    }
}