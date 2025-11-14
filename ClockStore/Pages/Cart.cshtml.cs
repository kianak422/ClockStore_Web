using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClockStore.Infrastructure;
using ClockStore.Models;

namespace ClockStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;

        public CartModel(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productID, string returnUrl)
        {
            Clock? clock = repository.Clocks
                .FirstOrDefault(p => p.ClockID == productID);
            if (clock != null)
            {
                Cart.AddItem(clock, 1);
            }
            return RedirectToPage("/Cart");
        }

        public IActionResult OnPostRemove(long clockID, string returnUrl)
        {
            ShoppingCartLine? line = Cart.Lines.FirstOrDefault(cl => cl.Clock?.ClockID == clockID);
            if (line?.Clock != null)
            {
                Cart.RemoveLine(line.Clock);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostUpdateQuantity(long clockID, int quantity, string action, string returnUrl)
        {
            Clock? clock = repository.Clocks.FirstOrDefault(p => p.ClockID == clockID);
            if (clock != null)
            {
                if (action == "increment")
                {
                    Cart.AddItem(clock, 1);
                }
                else if (action == "decrement")
                {
                    Cart.RemoveItem(clock, 1);
                }
                else if (quantity > 0)
                {
                    Cart.SetItemQuantity(clock, quantity);
                }
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostClear()
        {
            Cart.Clear();
            return RedirectToPage("/Cart");
        }
    }
}