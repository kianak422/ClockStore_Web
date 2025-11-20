using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

// Đặt namespace này phù hợp với thư mục gốc của dự án bạn
namespace ClockStore_Web.Components 
{
    // Model dùng để truyền danh sách menu ra View Component's View
    public class NavigationMenuViewModel 
    {
        // Cấu trúc (Text hiển thị, Tên Action, Tên Controller)
        public List<(string Text, string Action, string Controller)> MenuItems { get; set; } 
            = new List<(string, string, string)>();
    }

    [ViewComponent(Name = "NavigationMenu")] // Tên Component sẽ được dùng để gọi
    public class NavigationMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new NavigationMenuViewModel();
            
            // 1. Thêm các link luôn hiển thị
            model.MenuItems.Add(("Trang Chủ", "Index", "Home"));
            model.MenuItems.Add(("Sản Phẩm", "Index", "Product")); 
            
            // 2. LOGIC PHÂN QUYỀN ADMIN
            // Kiểm tra xem người dùng hiện tại có vai trò "Admin" hay không
            if (User.IsInRole("Admin")) 
            {
                // CHỈ THÊM LINK DASHBOARD NẾU LÀ ADMIN
                model.MenuItems.Add(("Dashboard", "Index", "Dashboard"));
            }
            
            // Trả về model để View Component's View có thể hiển thị
            return View(model);
        }
    }
}