using System.ComponentModel.DataAnnotations;

namespace ClockStore.Models
{
    public class ShoppingCartLine
    {
        public int ShoppingCartLineID { get; set; }

        public long OrderID { get; set; }
        public Order Order { get; set; }

        public long ClockID { get; set; }
        public Clock Clock { get; set; }

        public int Quantity { get; set; }
    }
}