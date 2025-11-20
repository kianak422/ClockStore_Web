using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ClockStore.Models
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Clock> Clocks => Set<Clock>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<ShoppingCartLine> ShoppingCartLines => Set<ShoppingCartLine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

