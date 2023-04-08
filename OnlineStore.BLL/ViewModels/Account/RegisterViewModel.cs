using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter Login")]
        [StringLength(30, ErrorMessage = "Login must be more than 8 and less than 30 characters long", MinimumLength = 6)]
        [EmailAddress(ErrorMessage = "This is not an Email Address")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Password")]
        [MinLength(8, ErrorMessage = "Password must be more than 8 characters long")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }
    }
}
