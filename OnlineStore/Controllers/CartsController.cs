using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.Interfaces;

namespace OnlineStore.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAccountsService _accountsService;
        private readonly IProductInfoService _productinfoService;
        private readonly IOrderService _orderService;

        public CartsController(ICartService cartService, IAccountsService accountsService, IProductInfoService productInfoService, IOrderService orderService)
        {
            _cartService = cartService;
            _accountsService = accountsService;
            _productinfoService = productInfoService;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> Index(int accountId)
        {
            var response = await _cartService.GetCart(accountId);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Index", "Products");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaymentPartialView()
        {
            var codeResponse = await _accountsService.CreateConfirmationCode();
            string message = string.Format($"<p>Your confirmation code: {codeResponse.Data}</p>");
            var sendEmailResponse = await _accountsService.SendEmail(User.Identity.Name, message, "Payment proof");

            if (sendEmailResponse.Data != true)
            {
                ModelState.AddModelError(string.Empty, sendEmailResponse.Description);

                return View();
            }

            var response = await _cartService.GetPaymentProofViewModel(codeResponse.Data);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                ViewBag.ConfirmationCode = codeResponse.Data;

                return PartialView("_PaymentProof");
            }

            return Json(new { success = false, status = response.StatusCode, description = response.Description });
        }

        [Authorize]
        public async Task<JsonResult> DeleteOrders()
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

            var productsResponse = await _productinfoService.ReduceAmount(ordersResponse.Data);

            if (!productsResponse.Data)
            {
                return Json(new { success = false, status = productsResponse.StatusCode, description = productsResponse.Description });
            }

            var cartResponse = await _cartService.ClearCart(accountResponse.Data.Cart.Id);

            return Json(new { success = cartResponse.Data, status = cartResponse.StatusCode, description = cartResponse.Description });
        }
    }
}
