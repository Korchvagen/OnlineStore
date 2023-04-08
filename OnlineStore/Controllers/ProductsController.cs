using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Products;

namespace OnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductInfoService _productInfoService;

        public ProductsController(IProductService productService, IProductInfoService productInfoService)
        {
            _productService = productService;
            _productInfoService = productInfoService;
        }

        public async Task<IActionResult> Index(FilterViewModel model)
        {
            var response = await _productService.GetProducts();
            CatalogViewModel cvm = new CatalogViewModel { Products = response.Data, Filter = new FilterViewModel() };

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return View(cvm);
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> ProductsFilter(FilterViewModel model)
        {
            var response = await _productService.GetFilteredProducts(model);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return PartialView("_Cards", response.Data);
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [HttpGet]
        public async Task<IActionResult> GetCardsPartialView()
        {
            var response = await _productService.GetProducts();

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return PartialView("_Cards", response.Data);
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPagePartialView(int id)
        {
            var response = await _productService.GetProductById(id);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return PartialView("_ProductPagePartialView", response.Data);
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        public async Task<IActionResult> ProductPage(int Id)
        {
            var response = await _productService.GetProductById(Id);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetCreatePartialView()
        {
            return PartialView("_ProductPopupForm", new CreateProductViewModel());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OpenPrevForm(int ProductId)
        {
            var response = await _productService.GetProductFormData(ProductId);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return PartialView("_ProductPopupForm", response.Data);
            }

            return new JsonResult(false);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            if(model.Image == null && model.Id != null)
            {
                ModelState.Remove("Image");
            }

            if (!ModelState.IsValid && model.Id == null)
            {
                model.Errors = ModelState.Select(m => m.Value.Errors).Where(d => d.Count > 0).ToList();

                return PartialView("_ProductPopupForm", model);
            }
            var response = model.Id != null
                ? await _productService.UpdateProductData(model)
                : await _productService.CreateProduct(model);

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return RedirectToAction("GetPartialView", "ProductsInfo", new { response.Data.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CloseModal(int id)
        {
            var response = await _productService.DeleteProduct(id);

            return Json(new { success = response.Data, status = response.StatusCode, description = response.Description });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetEditProductModal(int id)
        {
            var productResponse = await _productService.GetProductFormData(id);

            if (productResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = productResponse.StatusCode, description = productResponse.Description });
            }

            var productInfoResponse = await _productInfoService.GetProductInfoFormData(id);

            if (productInfoResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = productInfoResponse.StatusCode, description = productInfoResponse.Description });
            }

            FullProductViewModel fullProductViewModel = new FullProductViewModel()
            {
                Product = productResponse.Data,
                ProductInfo = productInfoResponse.Data
            };

            return PartialView("_EditProductPopup", fullProductViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(CreateProductViewModel model)
        {
            if (model.Image == null && model.PrevImage != null)
            {
                ModelState.Remove("Image");
            }

            if (!ModelState.IsValid)
            {
                model.Errors = ModelState.Select(m => m.Value.Errors).Where(d => d.Count > 0).ToList();

                return PartialView("_EditProductPopupForm", model);
            }

            var response = await _productService.UpdateProductData(model);

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return PartialView("_EditProductPopupForm", response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.GetProductById(id);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return PartialView("_ConfirmDeletionPopup", response.Data);
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ConfirmedDeletion(int id)
        {
            var response = await _productService.DeleteProduct(id);

            if (response.Data)
            {
                return RedirectToAction("GetCardsPartialView");
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [HttpPost]
        public async Task<IActionResult> GetSliderCard(int productId, bool isFromStart, int cardsNumber)
        {
            var response = await _productService.GetSliderProduct(productId, isFromStart, cardsNumber);

            if (response.StatusCode == DAL.Enum.StatusCode.LastProduct)
            {
                return Json(new { success = true, status = response.StatusCode, description = response.Description });
            }

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return PartialView("_SliderCard", response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetLastSliderCard()
        {
            var response = await _productService.GetLastSliderProduct(false);

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            return PartialView("_SliderCard", response.Data);
        }
    }
}
