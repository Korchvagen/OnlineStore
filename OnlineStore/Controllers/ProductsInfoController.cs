using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Products;

namespace OnlineStore.Controllers
{
    public class ProductsInfoController : Controller
    {
        private readonly IProductInfoService _productInfoService;

        public ProductsInfoController(IProductInfoService productInfoService)
        {
            _productInfoService = productInfoService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPartialView(int id)
        {
            var response = await _productInfoService.GetProductInfoFormData(id);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return PartialView("_ProductInfoPopupForm", response.Data);
            }

            CreateProductInfoViewModel product = new CreateProductInfoViewModel()
            {
                ProductId = id
            };

            return PartialView("_ProductInfoPopupForm", product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProductInfo(CreateProductInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Errors = ModelState.Select(m => m.Value.Errors).Where(d => d.Count > 0).ToList();

                return PartialView("_ProductInfoPopupForm", model);
            }

            var response = await _productInfoService.GetProductInfo(model.ProductId);
            var result = response.StatusCode == DAL.Enum.StatusCode.OK
                ? await _productInfoService.UpdateProductInfoData(model)
                : await _productInfoService.CreateProductInfo(model);

            if (result.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = result.StatusCode, description = result.Description });
            }

            return Json(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RememberProductInfoData(CreateProductInfoViewModel model)
        {
            var response = await _productInfoService.RememberProductInfoData(model);

            if (response.Data)
            {
                return RedirectToAction("OpenPrevForm", "Products", new { model.ProductId });
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditProductInfo(CreateProductInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Errors = ModelState.Select(m => m.Value.Errors).Where(d => d.Count > 0).ToList();

                return PartialView("_EditProductInfoPopupForm", model);
            }

            var response = await _productInfoService.UpdateProductInfoData(model);

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return PartialView("_EditProductInfoPopupForm", response.Data);
        }
    }
}
