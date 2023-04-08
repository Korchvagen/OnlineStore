#nullable disable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Account;
using OnlineStore.DAL.Enum;

namespace OnlineStore.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountsService _accountsService;
        private readonly ICartService _cartService;

        public AccountsController(IAccountsService accountsService, ICartService cartService)
        {
            _accountsService = accountsService;
            _cartService = cartService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var response = await _accountsService.GetAccounts();

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOut");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var LogInResponse = await _accountsService.LogIn(model);

            if (LogInResponse.StatusCode != DAL.Enum.StatusCode.OK && LogInResponse.StatusCode != DAL.Enum.StatusCode.AccountNotActivated)
            {
                ModelState.AddModelError(string.Empty, LogInResponse.Description);

                return View();
            }

            var claimsIdentityResponse = await _accountsService.Authentication(LogInResponse.Data);

            if (claimsIdentityResponse.StatusCode == DAL.Enum.StatusCode.OK && LogInResponse.StatusCode == DAL.Enum.StatusCode.AccountNotActivated)
            {
                string message = string.Format("To activate your account follow the link below: " +
                    "<a href=\"{0}\" title=\"Activate Account\">{0}</a>", Url.Action("ActivateAccount", "Accounts", new { login = model.Login }, Request.Scheme));
                var sendEmailResponse = await _accountsService.SendEmail(model.Login, message, "Account Activation");

                if(sendEmailResponse.Data != true)
                {
                    ModelState.AddModelError(string.Empty, sendEmailResponse.Description);

                    return View();
                }
            }

            if (claimsIdentityResponse.StatusCode == DAL.Enum.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentityResponse.Data));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, claimsIdentityResponse.Description);

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registerResponse = await _accountsService.Register(model);

            if (registerResponse.StatusCode != DAL.Enum.StatusCode.OK)
            {
                ModelState.AddModelError(string.Empty, registerResponse.Description);

                return View();
            }

            var cartResponse = await _cartService.CreateCart(registerResponse.Data);

            if (!cartResponse.Data)
            {
                ModelState.AddModelError(string.Empty, cartResponse.Description);

                return View();
            }
            var claimsIdentityResponse = await _accountsService.Authentication(registerResponse.Data);

            if (claimsIdentityResponse.StatusCode == DAL.Enum.StatusCode.OK)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentityResponse.Data));

                string message = string.Format("To activate your account follow the link below:" +
                    "<a href=\"{0}\" title=\"Activate Account\">{0}</a>", Url.Action("ActivateAccount", "Accounts",
                    new { login = model.Login }, Request.Scheme));
                var sendEmailResponse = await _accountsService.SendEmail(model.Login, message, "Account Activation");

                if (sendEmailResponse.Data != true)
                {
                    ModelState.AddModelError(string.Empty, sendEmailResponse.Description);

                    return View();
                }

                return RedirectToActionPermanent("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, claimsIdentityResponse.Description);

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("LogIn");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ActivateAccount(string login)
        {
            var response = await _accountsService.AccountActivation(login);

            if (response.Data)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult GetLogin()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetLogin(GetLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _accountsService.GetAccountByLogin(model.Login);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                var codeResponse = await _accountsService.CreateConfirmationCode();

                if(codeResponse.StatusCode != DAL.Enum.StatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, codeResponse.Description);

                    return View();
                }

                string message = string.Format($"<p>Your confirmation code: {codeResponse.Data}</p>");
                var sendEmailResponse = await _accountsService.SendEmail(response.Data.Login, message, "Password Renewal");

                if(sendEmailResponse.Data != true)
                {
                    ModelState.AddModelError(string.Empty, sendEmailResponse.Description);

                    return View();
                }

                CodeCheckViewModel codeCheckViewModel = new CodeCheckViewModel()
                {
                    Id = response.Data.Id,
                    Code = codeResponse.Data.ToString()
                };

                return View("CodeCheck", codeCheckViewModel);
            }

            ModelState.AddModelError(string.Empty, response.Description);

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CodeCheck(CodeCheckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            NewPasswordViewModel newPasswordViewModel = new NewPasswordViewModel()
            {
                Id = model.Id
            };

            return View("NewPassword", newPasswordViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _accountsService.NewPassword(model.Id, model.NewPassword);

            if (response.Data)
            {
                return RedirectToAction("LogIn");
            }

            ModelState.AddModelError(string.Empty, response.Description);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAccount(int id)
        {
            var response = await _accountsService.GetAccountByLogin(User.Identity.Name);

            if (response.StatusCode != DAL.Enum.StatusCode.OK)
            {
                return Json(new { success = false, status = response.StatusCode, description = response.Description });
            }

            if (response.Data.Status == AccountStatus.NotActivated)
            {
                return Json(new { success = false, status = DAL.Enum.StatusCode.AccountNotActivated, description = response.Description });
            }

            return RedirectToAction("CreateOrder", "Orders", new { productId = id, accountId = response.Data.Id });
        }

        [Authorize]
        public async Task<IActionResult> OpenCart()
        {
            var response = await _accountsService.GetAccountByLogin(User.Identity.Name);

            if (response.StatusCode == DAL.Enum.StatusCode.OK)
            {
                return RedirectToAction("Index", "Carts", new { accountId = response.Data.Id });
            }

            return RedirectToAction("Index", "Products");
        }

        public async Task<JsonResult> ChangeRole(int accountId, UserRole selectedRole)
        {
            var response = await _accountsService.ChangeRole(accountId, selectedRole);

            return Json(new { success = response.Data, status = response.StatusCode, description = response.Description });
        }
    }
}
