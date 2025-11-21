using ClockStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ClockStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCustomerController : Controller
    {
        private ICustomerRepository repository;
        private readonly ILogger<AdminCustomerController> _logger;

        public AdminCustomerController(ICustomerRepository repo, ILogger<AdminCustomerController> logger)
        {
            repository = repo;
            _logger = logger;
        }

        public ViewResult Index() => View(repository.Customers);

        public ViewResult Edit(long customerId) =>
            View(repository.Customers
                .FirstOrDefault(c => c.CustomerID == customerId));

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Calling SaveCustomer for customer: {customer.Name}");
                repository.SaveCustomer(customer);
                TempData["message"] = $"{customer.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogWarning($"ModelState is invalid for customer: {customer.Name}");
                // có lỗi với các giá trị dữ liệu
                return View(customer);
            }
        }

        public ViewResult Create() => View("Edit", new Customer { Name = "", Email = "", Address = "", PhoneNumber = "" });

        [HttpPost]
        public IActionResult Delete(long customerId)
        {
            Customer? deletedCustomer = repository.DeleteCustomer(customerId);
            if (deletedCustomer != null)
            {
                TempData["message"] = $"{deletedCustomer.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}