using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClockStore.Models;
using Microsoft.AspNetCore.Identity;

namespace ClockStore.Pages
{
    public class CheckoutModel : PageModel
    {
        private IOrderRepository repository;
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public CheckoutModel(IOrderRepository repoService, Cart cartService, UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
        {
            repository = repoService;
            Cart = cartService;
            Order = new Order();
            userManager = userMgr;
            signInManager = signInMgr;
        }

        public Cart Cart { get; set; }
        public Order Order { get; set; }

        public async Task OnGetAsync()
        {
            Order = new Order();

            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    Order.Name = user.UserName;
                    Order.Email = user.Email;
                    Order.PhoneNumber = user.PhoneNumber;
                }
            }
        }

        public IActionResult OnPost(Order order)
        {
            // Cart = cart;
            Order = order;

            if (Cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = Cart.Lines.ToArray();
                repository.SaveOrder(order);
                Cart.Clear();
                return RedirectToPage("OrderCompleted", new { orderId = order.OrderID });
            }
            else
            {
                return Page();
            }
        }
    }
}