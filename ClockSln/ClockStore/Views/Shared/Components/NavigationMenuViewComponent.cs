using Microsoft.AspNetCore.Mvc;
using ClockStore.Models;

namespace ClockStore.Components {
  public class NavigationMenuViewComponent : ViewComponent {
    private IStoreRepository repository;
    public NavigationMenuViewComponent(IStoreRepository repo) { repository = repo; }
    public IViewComponentResult Invoke() {
      ViewBag.SelectedCategory = RouteData?.Values["category"];
      return View(repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
    }
  }
}
