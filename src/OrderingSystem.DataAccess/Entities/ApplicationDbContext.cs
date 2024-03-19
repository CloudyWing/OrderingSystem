using Microsoft.EntityFrameworkCore;

namespace CloudyWing.OrderingSystem.DataAccess.Entities {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
               optionsBuilder.UseSqlServer("Server=127.0.0.1,1401;Database=OrderingSystem;User Id=sa;Password=Wing1205;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
