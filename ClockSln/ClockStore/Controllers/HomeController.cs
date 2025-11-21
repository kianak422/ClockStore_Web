using ClockStore.Models;
using ClockStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ClockStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreRepository repository;
        private readonly ILogger<HomeController> _logger;
        private int pageSize = 4;

        public HomeController(IStoreRepository repo, ILogger<HomeController> logger)
        {
            repository = repo;
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index(string category, int page = 1)
        {
            var clocks = repository.Clocks
                .Where(c => category == null || c.Category == category);

            var totalItems = clocks.Count();
            _logger.LogInformation($"Total Items: {totalItems}");

            var viewModel = new ProductsListViewModel
            {
                Products = clocks
                    .OrderBy(c => c.ClockID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                },
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}
