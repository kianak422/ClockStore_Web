namespace ClockStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreDbContext context;

        public EFStoreRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Clock> Clocks => context.Clocks;

        public void SaveClock(Clock clock)
        {
            if (clock.ClockID == 0)
            {
                context.Clocks.Add(clock);
            }
            else
            {
                Clock? dbEntry = context.Clocks.FirstOrDefault(p => p.ClockID == clock.ClockID);
                if (dbEntry != null)
                {
                    dbEntry.Name = clock.Name;
                    dbEntry.Description = clock.Description;
                    dbEntry.Price = clock.Price;
                    dbEntry.Category = clock.Category;
                }
            }
            context.SaveChanges();
        }

        public Clock? DeleteClock(long clockId)
        {
            Clock? dbEntry = context.Clocks.FirstOrDefault(p => p.ClockID == clockId);
            if (dbEntry != null)
            {
                context.Clocks.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public IQueryable<Customer> Customers => context.Customers;

        public void SaveCustomer(Customer customer)
        {
            if (customer.CustomerID == 0)
            {
                context.Customers.Add(customer);
            }
            else
            {
                Customer? dbEntry = context.Customers.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
                if (dbEntry != null)
                {
                    dbEntry.Name = customer.Name;
                    dbEntry.Email = customer.Email;
                    dbEntry.Address = customer.Address;
                }
            }
            context.SaveChanges();
        }

        public Customer? DeleteCustomer(long customerId)
        {
            Customer? dbEntry = context.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (dbEntry != null)
            {
                context.Customers.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
