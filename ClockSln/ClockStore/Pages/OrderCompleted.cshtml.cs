using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClockStore.Pages
{
    public class OrderCompletedModel : PageModel
    {
        public long OrderId { get; set; }

        public void OnGet(long orderId)
        {
            OrderId = orderId;
        }
    }
}