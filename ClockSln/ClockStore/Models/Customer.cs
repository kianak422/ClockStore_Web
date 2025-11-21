using System.ComponentModel.DataAnnotations;

namespace ClockStore.Models
{
    public class Customer
    {
        public long CustomerID { get; set; }

        [Required(ErrorMessage = "Please enter a customer name")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Please enter an address")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        public required string PhoneNumber { get; set; }
    }
}