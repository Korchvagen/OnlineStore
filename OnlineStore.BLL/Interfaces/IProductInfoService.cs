using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.ViewModels.Products;

namespace OnlineStore.BLL.Interfaces
{
    public interface IProductInfoService
    {
        Task<BaseResponse<CreateProductInfoViewModel>> CreateProductInfo(CreateProductInfoViewModel model);
        Task<BaseResponse<IEnumerable<Product>>> GetProducts();
        Task<BaseResponse<CreateProductInfoViewModel>> GetProductInfoFormData(int? id);
        Task<BaseResponse<bool>> RememberProductInfoData(CreateProductInfoViewModel model);
        Task<BaseResponse<CreateProductInfoViewModel>> UpdateProductInfoData(CreateProductInfoViewModel model);
        Task<BaseResponse<ProductInfo>> GetProductInfo(int id);
        Task<BaseResponse<bool>> ReduceAmount(List<Order> orders);
    }
}
