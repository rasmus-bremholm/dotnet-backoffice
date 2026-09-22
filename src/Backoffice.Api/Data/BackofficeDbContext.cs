using Backoffice.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Api.Data;

public class BackofficeDbContext : DbContext
{
   public BackofficeDbContext(DbContextOptions<BackofficeDbContext> options)
      : base(options)
   {

   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<AdminUser>()
          .HasIndex(u => u.Email)
          .IsUnique();
   }

   public DbSet<Product> Products { get; set; }
   public DbSet<Category> Categories { get; set; }
   public DbSet<VatRate> VatRates { get; set; }
   public DbSet<Brand> Brands { get; set; }
   public DbSet<AdminUser> AdminUsers { get; set; }
}
