using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.BLL.ViewModels.Products
{
    public class CreateProductInfoViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Enter material")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Value must be between 5 and 20 characters long")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Enter color")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Value must be between 4 and 20 characters long")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Enter memory")]
        [RegularExpression("^([0-9]{1,4}\\s(GB|TB))$", ErrorMessage = "Value should look like 32 GB or 1 TB")]
        public string Memory { get; set; }

        [Required(ErrorMessage = "Enter amount")]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Enter creation date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Enter life time")]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        public int LifeTime { get; set; }

        [Required(ErrorMessage = "Enter rating")]
        [RegularExpression("^([0-9](\\,[0-9])?|10)$", ErrorMessage = "Value must be between 0 and 10 and may include a fractional part")]
        public string Rating { get; set; }

        public List<ModelErrorCollection>? Errors { get; set; }
    }
}
