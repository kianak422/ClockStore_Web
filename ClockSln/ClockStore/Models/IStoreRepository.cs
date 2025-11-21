namespace ClockStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Clock> Clocks { get; }
        void SaveClock(Clock clock);
        Clock? DeleteClock(long clockId);

        IQueryable<Customer> Customers { get; }
        void SaveCustomer(Customer customer);
        Customer? DeleteCustomer(long customerId);
    }
}
