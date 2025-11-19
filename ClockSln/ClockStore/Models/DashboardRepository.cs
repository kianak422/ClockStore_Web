using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClockStore.Models
{
    // 1. Tạo Interface
    public interface IDashboardRepository
    {
        int GetTotalOrders();
        decimal GetTotalRevenue();
        Task<IEnumerable<object>> GetSalesByPeriodAsync(); // Cho biểu đồ Doanh thu
        Task<IEnumerable<object>> GetTopProductsAsync();   // Cho biểu đồ Top sản phẩm
        IEnumerable<Order> GetRecentOrders();              // Cho bảng đơn hàng mới
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

        public async Task<IEnumerable<object>> GetSalesByPeriodAsync()
        {
            // Thống kê doanh thu 7 ngày gần nhất có đơn
            var data = await _context.Orders
                .Include(o => o.Lines)
                .ThenInclude(l => l.Clock)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd/MM"),
                    Revenue = g.Sum(o => o.Lines.Sum(l => (l.Clock.Price) * l.Quantity))
                })
                .OrderBy(x => x.Date)
                .Take(7)
                .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<object>> GetTopProductsAsync()
        {
            // Top 5 sản phẩm bán chạy nhất
            var data = await _context.Orders
                .SelectMany(o => o.Lines)
                .Include(l => l.Clock)
                .GroupBy(l => l.Clock.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    Quantity = g.Sum(l => l.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

            return data;
        }

        public IEnumerable<Order> GetRecentOrders()
        {
            // Lấy 5 đơn hàng mới nhất
            return _context.Orders
                .Include(o => o.Lines)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();
        }
    }
}