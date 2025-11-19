using System.ComponentModel.DataAnnotations.Schema;

namespace ClockStore.Models
{
    public class Clock
    {
        public long? ClockID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}

