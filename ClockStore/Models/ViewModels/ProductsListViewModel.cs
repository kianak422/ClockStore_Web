using System.Collections.Generic;

namespace ClockStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Clock> Products { get; set; } = new List<Clock>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public string? CurrentCategory { get; set; }
    }
}