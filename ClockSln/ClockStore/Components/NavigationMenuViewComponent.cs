using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ClockStore.Models;

namespace ClockStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Clocks
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
        
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
