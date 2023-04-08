using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Banner
{
    public class EditBannerViewModel
    {
        public int Id { get; set; }

        [RegularExpression("^\\/[a-zA-Z]{1,12}\\/[a-zA-Z]{1,20}(\\/[0-9]{1,3})?$", ErrorMessage = "Link should look like /Controller/Action(/id)?")]
        public string NewLink { get; set; }

        public IFormFile? NewImage { get; set; }

        public List<ModelErrorCollection>? Errors { get; set; }
    }
}
