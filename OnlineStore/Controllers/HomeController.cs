using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Home;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAccountsService _accountsService;
        private readonly IBannerService _bannerService;

        public HomeController(IProductService productService, IAccountsService accountsService, IBannerService bannerService)
        {
            _productService = productService;
            _accountsService = accountsService;
            _bannerService = bannerService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var accountResponse = await _accountsService.GetAccountByLogin(User.Identity.Name);
            var productResponse = await _productService.GetLastSliderProduct(true);
            var bannerResponse = await _bannerService.GetFirstBanner();

            if(productResponse.StatusCode == DAL.Enum.StatusCode.OK)
            {
                HomeViewModel homeViewModel = new HomeViewModel()
                {
                    Account = accountResponse.Data,
                    Product = productResponse.Data,
                    Banner = bannerResponse.Data
                };

                return View(homeViewModel);
            }

            return View();
        }
    }
}