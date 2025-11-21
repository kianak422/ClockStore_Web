using System.Collections.Generic;

namespace ClockStore.Models.ViewModels
{
    public class ReportViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public required IEnumerable<SalesByPeriodViewModel> SalesByPeriod { get; set; }
        public required IEnumerable<TopProductsViewModel> TopProducts { get; set; }
    }
}