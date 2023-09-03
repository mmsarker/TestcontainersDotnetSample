using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;

namespace ProductManagement.DataAccess
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());
        }

        public DbSet<Product> Products { get; set; }
    }
}