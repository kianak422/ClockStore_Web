using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClockStore.Models
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private StoreDbContext _context;
        private readonly ILogger<EFCustomerRepository> _logger;

        public EFCustomerRepository(StoreDbContext ctx, ILogger<EFCustomerRepository> logger)
        {
            _context = ctx;
            _logger = logger;
        }

        public IQueryable<Customer> Customers => _context.Customers;

        public void SaveCustomer(Customer customer)
        {
            _logger.LogInformation($"Attempting to save customer: {customer.Name}");
            try
            {
                // Kiểm tra email hoặc số điện thoại trùng lặp
                bool isDuplicate = false;
                if (customer.CustomerID == 0) // Khách hàng mới
                {
                    isDuplicate = _context.Customers.Any(c =>
                        (c.Email != null && c.Email == customer.Email) ||
                        (c.PhoneNumber != null && c.PhoneNumber == customer.PhoneNumber));
                }
                else // Khách hàng hiện có đang được cập nhật
                {
                    isDuplicate = _context.Customers.Any(c =>
                        c.CustomerID != customer.CustomerID &&
                        ((c.Email != null && c.Email == customer.Email) ||
                         (c.PhoneNumber != null && c.PhoneNumber == customer.PhoneNumber)));
                }

                if (isDuplicate)
                {
                    _logger.LogWarning($"Attempt to save duplicate customer: {customer.Name} with Email: {customer.Email} or Phone: {customer.PhoneNumber}. Save aborted.");
                    return; // Hủy bỏ thao tác lưu
                }

                if (customer.CustomerID == 0)
                {
                    _context.Customers.Add(customer);
                }
                else
                {
                    Customer? dbEntry = _context.Customers.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = customer.Name;
                        dbEntry.Address = customer.Address;
                        dbEntry.PhoneNumber = customer.PhoneNumber;
                        dbEntry.Email = customer.Email;
                    }
                }
                _context.SaveChanges();
                _logger.LogInformation($"Customer {customer.Name} saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving customer {customer.Name}.");
                throw; // Ném lại ngoại lệ sau khi ghi nhật ký
            }
        }

        public Customer? DeleteCustomer(long customerId)
        {
            Customer? dbEntry = _context.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (dbEntry != null)
            {
                _context.Customers.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}