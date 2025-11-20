using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClockStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext context;

        public EFOrderRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Clock);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Clock).Where(c => c != null).Cast<Clock>());
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}