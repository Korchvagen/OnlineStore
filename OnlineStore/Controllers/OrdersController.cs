using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.Interfaces;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderService _orderService;
        private IProductService _productService;
        private ICartService _cartService;
        private IAccountsService _accountsService;

        public OrdersController(IOrderService orderService, IProductService productService, ICartService cartService, IAccountsService accountsService)
        {
            _orderService = orderService;
            _productService = productService;
            _cartService = cartService;
            _accountsService = accountsService;
        }

        public async Task<JsonResult> CreateOrder(int productId, int accountId)
        {
            var productResponse = await _productService.GetProductById(productId);

            if (productResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = productResponse.StatusCode, description = productResponse.Description });
            }

            var cartResponse = await _cartService.GetCart(accountId);

            if (cartResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = cartResponse.StatusCode, description = cartResponse.Description });
            }

            var orderResponse = await _orderService.CreateOrder(productResponse.Data, cartResponse.Data);

            return Json(new { success = orderResponse.Data, status = orderResponse.StatusCode, description = orderResponse.Description });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteOrder(id);

            if (response.Data)
            {
                return RedirectToAction("GetOrdersPartialView");
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        public async Task<IActionResult> GetOrdersPartialView()
        {
            var accountResponse = await _accountsService.GetAccountByLogin(User.Identity.Name);

            if (accountResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = accountResponse.StatusCode, description = accountResponse.Description });
            }

            var ordersResponse = await _orderService.GetOrders(accountResponse.Data.Cart.Id);

            if (ordersResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = ordersResponse.StatusCode, description = ordersResponse.Description });
            }

            return PartialView("_ProductCartList", ordersResponse.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrdersAmount(int id, int ordersAmount, bool isIncrease)
        {
            var response = await _orderService.ChangeOrdersAmount(id, ordersAmount, isIncrease);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetOrdersPartialView");
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrdersAmountByInput(int id, int ordersAmount)
        {
            var response = await _orderService.ChangeOrdersAmountByInput(id, ordersAmount);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetOrdersPartialView");
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }
    }
}
