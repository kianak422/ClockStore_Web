using ClockStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClockStore.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ Admin mới vào được
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _repo;

        public DashboardController(IDashboardRepository repo)
        {
            _repo = repo;
        }

        // GET: /Dashboard/Index
        public IActionResult Index()
        {
            // Truyền số liệu cơ bản sang View (để thay thế số 1,234 giả)
            ViewBag.TotalOrders = _repo.GetTotalOrders();
            ViewBag.TotalRevenue = _repo.GetTotalRevenue();
            ViewBag.RecentOrders = _repo.GetRecentOrders();
            
            return View();
        }

        // API: /Dashboard/GetChartData
        // Frontend sẽ gọi cái này để vẽ biểu đồ
        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            var sales = await _repo.GetSalesByPeriodAsync();
            var products = await _repo.GetTopProductsAsync();

            return Json(new { salesData = sales, productData = products });
        }
    }
}