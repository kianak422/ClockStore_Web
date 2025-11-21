using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ClockStore.Models.ViewModels;
using System.Threading.Tasks;

namespace ClockStore.Models
{
    // 1. Tạo Interface
    public interface IDashboardRepository
    {
        int GetTotalOrders();
        decimal GetTotalRevenue();
        IEnumerable<SalesByPeriodViewModel> GetSalesByPeriodAsync(); // Cho biểu đồ Doanh thu
        Task<IEnumerable<TopProductsViewModel>> GetTopProductsAsync();   // Cho biểu đồ Top sản phẩm
        IEnumerable<RecentOrderViewModel> GetRecentOrders();              // Cho bảng đơn hàng mới
    }

    // 2. Viết code xử lý (Implement)
    public class DashboardRepository : IDashboardRepository
    {
        private readonly StoreDbContext _context;

        public DashboardRepository(StoreDbContext context)
        {
            _context = context;
        }

        public int GetTotalOrders()
        {
            return _context.Orders.Count();
        }

        public decimal GetTotalRevenue()
        {
            // Lấy tất cả đơn hàng và tính tổng (Giá * Số lượng)
            var orders = _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Clock)
                .ToList(); // Tải về RAM để tính toán cho an toàn

            return orders.Sum(o => o.Lines.Sum(l => (l.Clock?.Price ?? 0) * l.Quantity));
        }

        public IEnumerable<SalesByPeriodViewModel> GetSalesByPeriodAsync()
        {
            // Thống kê doanh thu 7 ngày gần nhất có đơn
            var data = _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Clock)
                .GroupBy(o => o.OrderDate.Date)
                .AsEnumerable() // Force client-side evaluation for ToString
                .Select(g => new SalesByPeriodViewModel
                {
                    Date = g.Key.ToString("dd/MM"),
                    Revenue = g.Sum(o => o.Lines.Where(l => l.Clock != null).Sum(l => l.Clock!.Price * l.Quantity))
                })
                .OrderBy(x => x.Date)
                .Take(7)
                .ToList();

            return data;
        }

        public async Task<IEnumerable<TopProductsViewModel>> GetTopProductsAsync()
        {
            // Top 5 sản phẩm bán chạy nhất
            var data = await _context.Orders
                .SelectMany(o => o.Lines)
                .Include(l => l.Clock)
                .Where(l => l.Clock != null)
                .GroupBy(l => l.Clock!.Name)
                .Select(g => new TopProductsViewModel
                {
                    ProductName = g.Key,
                    Quantity = g.Sum(l => l.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

            return data;
        }

        public IEnumerable<RecentOrderViewModel> GetRecentOrders()
        {
            // Lấy 5 đơn hàng mới nhất và tính tổng tiền cho mỗi đơn hàng
            return _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Clock)
                .Where(o => o != null) // Explicitly filter out null orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new RecentOrderViewModel
                {
                    Order = o,
                    Total = o.Lines.Sum(l => (l.Clock != null ? l.Clock.Price : 0) * l.Quantity)
                })
                .ToList();
        }
    }
}