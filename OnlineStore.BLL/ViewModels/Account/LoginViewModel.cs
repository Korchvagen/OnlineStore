using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Login")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
    }
}
