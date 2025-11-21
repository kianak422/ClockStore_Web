using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClockStore.Models;
using Microsoft.AspNetCore.Identity;

namespace ClockStore.Pages
{
    public class CheckoutModel : PageModel
    {
        private IOrderRepository repository;
        private ICustomerRepository customerRepository;
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public CheckoutModel(IOrderRepository repoService, Cart cartService, UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signInMgr, ICustomerRepository custRepo)
        {
            repository = repoService;
            Cart = cartService;
            Order = new Order();
            userManager = userMgr;
            signInManager = signInMgr;
            customerRepository = custRepo;
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
            // Giỏ hàng = giỏ hàng;
            Order = order;

            if (Cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                // Tạo đối tượng Customer mới từ thông tin đơn hàng
                Customer customer = new Customer
                {
                    Name = order.Name,
                    Address = $"{order.Line1}, {order.Line2}, {order.Line3}, {order.City}, {order.State}",
                    PhoneNumber = order.PhoneNumber,
                    Email = order.Email
                };
                customerRepository.SaveCustomer(customer);

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