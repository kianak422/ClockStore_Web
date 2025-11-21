using ClockStore.Models.ViewModels;
using ClockStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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
        public async Task<IActionResult> Index()
        {
            // Truyền số liệu cơ bản sang View (để thay thế số 1,234 giả)
            ViewBag.TotalOrders = _repo.GetTotalOrders();
            ViewBag.TotalRevenue = _repo.GetTotalRevenue();
            try
            {
                ViewBag.RecentOrders = _repo.GetRecentOrders();
            }
            catch (Exception ex)
            {
                // Log the exception (for debugging purposes)
                // You might want to use a proper logging framework here
                Console.WriteLine($"Error loading recent orders: {ex.Message}");
                ViewBag.RecentOrders = new List<RecentOrderViewModel>(); // Ensure it's not null
            }
            ViewBag.TopProducts = await _repo.GetTopProductsAsync();
            
            return View("~/Views/Home/Dashboard.cshtml");
        }

        // API: /Dashboard/GetChartData
        // Frontend sẽ gọi cái này để vẽ biểu đồ
        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            var sales = _repo.GetSalesByPeriodAsync();
            var products = await _repo.GetTopProductsAsync();

            return Json(new { salesData = sales, productData = products });
        }
    }
}