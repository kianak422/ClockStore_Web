using System.ComponentModel.DataAnnotations;

namespace ClockStore.Models
{
    public class LoginModel
    {
        [Required]
        public required string Name { get; set; }
        public required string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public required string ConfirmPassword { get; set; }
    }
}