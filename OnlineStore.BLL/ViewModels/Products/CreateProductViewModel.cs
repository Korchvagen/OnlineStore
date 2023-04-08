using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OnlineStore.DAL.Enum;

namespace OnlineStore.BLL.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Name must be between 8 and 30 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Choose Categoty")]
        public ProductCategory Category { get; set; }

        [Required(ErrorMessage = "Enter Price")]
        [RegularExpression("^([0-9]{1,6}(\\,[0-9]{1,2})?)$", ErrorMessage = "Wrong price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Choose Image")]
        public IFormFile Image { get; set; }

        public byte[]? PrevImage { get; set; }

        public string? FileName { get; set; }

        public List<ModelErrorCollection>? Errors { get; set; }
    }
}
