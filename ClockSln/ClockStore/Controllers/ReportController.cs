using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClockStore.Models;
using ClockStore.Models.ViewModels;
using System.Threading.Tasks;

namespace ClockStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private IDashboardRepository _dashboardRepository;

        public ReportController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportViewModel
            {
                TotalOrders = _dashboardRepository.GetTotalOrders(),
                TotalRevenue = _dashboardRepository.GetTotalRevenue(),
                SalesByPeriod = _dashboardRepository.GetSalesByPeriodAsync(),
                TopProducts = await _dashboardRepository.GetTopProductsAsync()
            };
            return View(viewModel);
        }
    }
}