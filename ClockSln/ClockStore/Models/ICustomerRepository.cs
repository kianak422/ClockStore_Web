using System.Linq;

namespace ClockStore.Models
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> Customers { get; }
        void SaveCustomer(Customer customer);
        Customer? DeleteCustomer(long customerId);
    }
}