using Microsoft.EntityFrameworkCore;

namespace ClockStore.Models {
  public static class SeedData {
    public static void EnsurePopulated(IApplicationBuilder app) {
      StoreDbContext context = app.ApplicationServices.CreateScope()
        .ServiceProvider.GetRequiredService<StoreDbContext>();

      if (context.Database.GetPendingMigrations().Any()) {
        context.Database.Migrate();
      }

      if (!context.Products.Any()) {
        context.Products.AddRange(
          new Product { Name = "Classic Leather", Description = "Analog watch with leather strap", Category = "Analog", Price = 125 },
          new Product { Name = "Sport Runner", Description = "Water-resistant sport watch", Category = "Sport", Price = 90 },
          new Product { Name = "Smart Lite", Description = "Entry-level smartwatch", Category = "Smart", Price = 199.99m },
          new Product { Name = "Luxury Gold", Description = "Premium gold-plated watch", Category = "Luxury", Price = 1299.50m }
          // thêm nhiều item tuỳ ý...
        );
        context.SaveChanges();
      }
    }
  }
}
