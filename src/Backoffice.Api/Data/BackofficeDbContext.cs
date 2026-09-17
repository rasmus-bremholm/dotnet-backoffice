using Backoffice.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Api.Data;

public class BackofficeDbContext : DbContext
{
   public BackofficeDbContext(DbContextOptions<BackofficeDbContext> options)
      : base(options)
   {

   }

   public DbSet<Product> Products { get; set; }
   public DbSet<Category> Categories { get; set; }
}
