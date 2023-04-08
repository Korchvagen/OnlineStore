using OnlineStore.DAL.Models;

namespace OnlineStore.BLL.ViewModels.Products
{
    public class CatalogViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public FilterViewModel Filter { get; set; }
    }
}
