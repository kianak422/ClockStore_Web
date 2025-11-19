namespace ClockStore.Models
{
    public class ShoppingCartLine
    {
        public int CartLineID { get; set; }
        public Clock? Clock { get; set; }
        public int Quantity { get; set; }
    }
}