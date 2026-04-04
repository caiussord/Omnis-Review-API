using CCSS_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CCSS_API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("Users");
            b.Property(u => u.Id)
                .HasColumnName("User_Id")
                .HasDefaultValueSql("NEWID()")
                .IsRequired();
                
            b.Property(u => u.Name)
                .HasMaxLength(500)
                .IsRequired();

            b.Property(u => u.Birth_Date)
                .HasColumnType("date")
                .IsRequired();

            b.Property(u => u.PasswordHash).HasColumnName("Password").HasMaxLength(255);
        });
    }
}
