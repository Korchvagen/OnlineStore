using OnlineStore.BLL.ViewModels.Banner;
using OnlineStore.DAL.Response;

namespace OnlineStore.BLL.Interfaces
{
    public interface IBannerService
    {
        Task<BaseResponse<IEnumerable<BannerViewModel>>> GetBanners();

        Task<BaseResponse<BannerViewModel>> GetFirstBanner();

        Task<BaseResponse<bool>> CreateBanner(CreateBannerViewModel model);

        Task<BaseResponse<bool>> UpdateBanner(EditBannerViewModel model);

        Task<BaseResponse<bool>> DeleteBanner(int id);
    }
}
