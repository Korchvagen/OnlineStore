using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Banner;
using System.Data;

namespace OnlineStore.Controllers
{
    public class BannersController : Controller
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBanners()
        {
            var response = await _bannerService.GetBanners();

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return Json(new { success = true, data = response.Data, status = response.StatusCode });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetEditBannersPartialView()
        {
            var response = await _bannerService.GetBanners();

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return PartialView("_EditBannersPartialView", response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var response = await _bannerService.DeleteBanner(id);

            return Json(new { success = response.Data, status = response.StatusCode, description = response.Description });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetCreateBannerPartialView()
        {
            return PartialView("_CreateBannerPartialView", new CreateBannerViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBanner(CreateBannerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Errors = ModelState.Select(m => m.Value.Errors).Where(d => d.Count > 0).ToList();

                return PartialView("_CreateBannerPartialView", model);
            }

            var response = await _bannerService.CreateBanner(model);

            return Json(new { success = response.Data, status = response.StatusCode, description = response.Description });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditBanner(EditBannerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Errors = ModelState.Select(m => m.Value.Errors).Where(d => d.Count > 0).ToList();

                return PartialView("_EditBannerFormItemPartialView", model);
            }

            var response = await _bannerService.UpdateBanner(model);

            return Json(new { success = response.Data, status = response.StatusCode, description = response.Description });
        }
    }
}
