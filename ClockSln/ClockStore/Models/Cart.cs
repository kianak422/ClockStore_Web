using System.Collections.Generic;
using System.Linq;

namespace ClockStore.Models
{
    public class Cart
    {
        public List<ShoppingCartLine> Lines { get; set; } = new List<ShoppingCartLine>();

        public virtual void AddItem(Clock clock, int quantity)
        {
            ShoppingCartLine? line = Lines
                .Where(p => p.Clock != null && p.Clock.ClockID == clock.ClockID)
                .FirstOrDefault();

            if (line == null)
            {
                Lines.Add(new ShoppingCartLine
                {
                    Clock = clock,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveItem(Clock clock, int quantity = 1)
        {
            ShoppingCartLine? line = Lines
                .Where(p => p.Clock != null && p.Clock.ClockID == clock.ClockID)
                .FirstOrDefault();

            if (line != null)
            {
                line.Quantity -= quantity;
                if (line.Quantity <= 0)
                {
                    Lines.Remove(line);
                }
            }
        }

        public virtual void SetItemQuantity(Clock clock, int quantity)
        {
            ShoppingCartLine? line = Lines
                .Where(p => p.Clock != null && p.Clock.ClockID == clock.ClockID)
                .FirstOrDefault();

            if (line != null)
            {
                line.Quantity = quantity;
                if (line.Quantity <= 0)
                {
                    Lines.Remove(line);
                }
            }
        }

        public virtual void RemoveLine(Clock clock)
        {
            Lines.RemoveAll(l => l.Clock != null && l.Clock.ClockID == clock.ClockID);
        }

        public virtual decimal ComputeTotalValue()
        {
            return Lines.Sum(e => e.Clock != null ? e.Clock.Price * e.Quantity : 0);
        }

        public virtual void Clear()
        {
            Lines.Clear();
        }
    }
}