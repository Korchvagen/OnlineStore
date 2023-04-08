using OnlineStore.DAL.Enum;

namespace OnlineStore.BLL.ViewModels.Products
{
    public class FilterViewModel
    {
        public string? Price { get; set;}

        public ProductCategory Category { get; set;}

        public string? Name { get; set;}

        public bool Availability { get; set;}
    }
}
