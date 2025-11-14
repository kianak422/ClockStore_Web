using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ClockStore.Models
{
    public static class SeedData
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            // Tạo vai trò "Admin" nếu chưa tồn tại
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Tạo người dùng admin nếu chưa tồn tại và gán vai trò "Admin"
            if (await userManager.FindByNameAsync("Admin") == null)
            {
                ApplicationUser adminUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@example.com",
                FullName = "Admin User"
            };
                IdentityResult result = await userManager.CreateAsync(adminUser, "Admin@123"); // Mật khẩu mặc định
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Xóa tất cả các đơn hàng và các mục trong giỏ hàng trước khi thêm dữ liệu mới
            if (context.Orders.Any())
            {
                context.Orders.RemoveRange(context.Orders);
                context.SaveChanges();
            }

            // Xóa tất cả các sản phẩm hiện có trước khi thêm dữ liệu mới
            if (context.Clocks.Any())
            {
                context.Clocks.RemoveRange(context.Clocks);
                context.SaveChanges();
            }

            if (!context.Clocks.Any())
            {
                context.Clocks.AddRange(
                    new Clock { Name = "Classic Wall Clock", Description = "Elegant wood frame", Category = "Wall", Price = 450000m },
                    new Clock { Name = "Digital Alarm Clock", Description = "LED display, multiple alarms", Category = "Alarm", Price = 250000m },
                    new Clock { Name = "Cuckoo Clock", Description = "Handcrafted, authentic", Category = "Decor", Price = 1250000m },
                    new Clock { Name = "Minimal Table Clock", Description = "Modern, compact", Category = "Table", Price = 350000m },
                    new Clock { Name = "Grandfather Clock", Description = "Tall, pendulum clock", Category = "Floor", Price = 5000000m },
                    new Clock { Name = "Travel Alarm Clock", Description = "Compact, battery-powered", Category = "Alarm", Price = 180000m },
                    new Clock { Name = "Vintage Desk Clock", Description = "Retro design, brass finish", Category = "Table", Price = 600000m },
                    new Clock { Name = "Kitchen Wall Clock", Description = "Large, easy-to-read", Category = "Wall", Price = 300000m },
                    new Clock { Name = "Smart Alarm Clock", Description = "Voice control, smart features", Category = "Alarm", Price = 750000m },
                    new Clock { Name = "Art Deco Clock", Description = "Geometric design, unique", Category = "Decor", Price = 1500000m },
                    new Clock { Name = "Wooden Mantel Clock", Description = "Traditional, chimes", Category = "Table", Price = 900000m },
                    new Clock { Name = "Outdoor Wall Clock", Description = "Weather-resistant, large display", Category = "Wall", Price = 550000m },
                    new Clock { Name = "Projection Alarm Clock", Description = "Projects time on ceiling", Category = "Alarm", Price = 400000m },
                    new Clock { Name = "Modern Grandfather Clock", Description = "Sleek design, silent", Category = "Floor", Price = 6000000m },
                    new Clock { Name = "Digital Photo Frame Clock", Description = "Displays photos and time", Category = "Table", Price = 800000m },
                    new Clock { Name = "Atomic Wall Clock", Description = "Self-setting, accurate", Category = "Wall", Price = 700000m },
                    new Clock { Name = "Sunrise Alarm Clock", Description = "Simulates sunrise for waking", Category = "Alarm", Price = 650000m },
                    new Clock { Name = "Industrial Wall Clock", Description = "Metal frame, rustic look", Category = "Wall", Price = 480000m },
                    new Clock { Name = "Kids Alarm Clock", Description = "Fun design, easy to use", Category = "Alarm", Price = 200000m },
                    new Clock { Name = "Luxury Desk Clock", Description = "High-end materials, elegant", Category = "Table", Price = 1800000m },
                    new Clock { Name = "Minimalist Wall Clock", Description = "Simple design, clean lines", Category = "Wall", Price = 380000m },
                    new Clock { Name = "Voice Activated Alarm Clock", Description = "Responds to voice commands", Category = "Alarm", Price = 500000m }
                );
                context.SaveChanges();
            }
        }
    }
}

