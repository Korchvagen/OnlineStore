using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Account
{
    public class GetLoginViewModel
    {
        [Required(ErrorMessage = "Enter Login")]
        public string Login { get; set; }
    }
}
