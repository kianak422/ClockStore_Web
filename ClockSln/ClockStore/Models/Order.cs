using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System; // <--- Nhớ thêm dòng này

namespace ClockStore.Models
{
    public class Order
    {
        [BindNever]
        public long OrderID { get; set; }

        // --- THÊM DÒNG NÀY VÀO ---
        [BindNever]
        public DateTime OrderDate { get; set; } = DateTime.Now; 
        // --------------------------

        [BindNever]
        public ICollection<ShoppingCartLine> Lines { get; set; } = new List<ShoppingCartLine>();

        [Required(ErrorMessage = "Please enter a name")]
        public string? Name { get; set; }

        // ... (Giữ nguyên các phần còn lại: Line1, City, Email...) ...
        [Required(ErrorMessage = "Please enter the first address line")]
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Please enter a state name")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        public string? Email { get; set; }

        public bool GiftWrap { get; set; }

        [BindNever]
        public bool Shipped { get; set; }
    }
}