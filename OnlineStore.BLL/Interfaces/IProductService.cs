using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.ViewModels.Products;

namespace OnlineStore.BLL.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<IEnumerable<Product>>> GetProducts();
        Task<BaseResponse<Product>> GetProductById(int id);
        Task<BaseResponse<IEnumerable<Product>>> GetFilteredProducts(FilterViewModel model);
        Task<BaseResponse<CreateProductViewModel>> CreateProduct(CreateProductViewModel model);
        Task<BaseResponse<CreateProductViewModel>> GetProductFormData(int id);
        Task<BaseResponse<CreateProductViewModel>> UpdateProductData(CreateProductViewModel model);
        Task<BaseResponse<bool>> DeleteProduct(int id);
        Task<BaseResponse<Product>> GetSliderProduct(int productId, bool isFromStart, int cardsNumber);
        Task<BaseResponse<Product>> GetLastSliderProduct(bool isFromStart);
    }
}
