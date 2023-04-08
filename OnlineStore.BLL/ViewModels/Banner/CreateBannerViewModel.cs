using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Banner
{
    public class CreateBannerViewModel
    {
        [Required(ErrorMessage = "Enter link")]
        [RegularExpression("^\\/[a-zA-Z]{1,12}\\/[a-zA-Z]{1,20}(\\/[0-9]{1,3})?$", ErrorMessage = "Link should look like /Controller/Action(/id)?")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Choose image")]
        public IFormFile NewImage { get; set; }

        public List<ModelErrorCollection>? Errors { get; set; }
    }
}
