using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Account
{
    public class NewPasswordViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter New Password")]
        [MinLength(8, ErrorMessage = "Password must be more than 8 characters long")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set;}
    }
}
