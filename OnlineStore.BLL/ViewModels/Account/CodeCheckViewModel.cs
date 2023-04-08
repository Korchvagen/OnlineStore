using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Account
{
    public class CodeCheckViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }


        [Required(ErrorMessage = "Enter Code")]
        [Compare("Code", ErrorMessage = "Wrong code")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Wrong code")]
        public string CodeConfirm { get; set; }
    }
}
