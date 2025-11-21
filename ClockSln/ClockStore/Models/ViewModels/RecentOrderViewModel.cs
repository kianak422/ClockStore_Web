namespace ClockStore.Models.ViewModels
{
    public class RecentOrderViewModel
    {
        public required Order Order { get; set; }
        public decimal Total { get; set; }
    }
}