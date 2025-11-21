using ClockStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClockStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
    {
        private IStoreRepository repository;

        public AdminProductController(IStoreRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Clocks);

        public ViewResult Edit(long clockId) =>
            View(repository.Clocks
                .FirstOrDefault(p => p.ClockID == clockId));

        [HttpPost]
        public IActionResult Edit(Clock clock)
        {
            if (ModelState.IsValid)
            {
                repository.SaveClock(clock);
                TempData["message"] = $"{clock.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(clock);
            }
        }

        public ViewResult Create() => View("Edit", new Clock());

        [HttpPost]
        public IActionResult Delete(long clockId)
        {
            Clock? deletedClock = repository.DeleteClock(clockId);
            if (deletedClock != null)
            {
                TempData["message"] = $"{deletedClock.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}