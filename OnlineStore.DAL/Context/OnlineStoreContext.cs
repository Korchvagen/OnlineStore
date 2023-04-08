using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Models;

namespace OnlineStore.DAL.Context
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OnlineStoreContext-cbe05883-b90b-4e94-8695-a2be40a0174b;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public DbSet<Accounts> Accounts { get; set; } = default!;

        public DbSet<Product> Product { get; set; } = default!;

        public DbSet<ProductInfo> ProductInfo { get; set; } = default!;

        public DbSet<Cart> Cart { get; set; } = default!;

        public DbSet<Order> Order { get; set; } = default!;

        public DbSet<Banner> Banner { get; set; } = default!;
    }
}
