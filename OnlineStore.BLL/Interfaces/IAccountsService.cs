using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.ViewModels.Account;
using System.Security.Claims;
using OnlineStore.DAL.Enum;

namespace OnlineStore.BLL.Interfaces
{
    public interface IAccountsService
    {
        Task<BaseResponse<IEnumerable<AccountViewModel>>> GetAccounts();
        Task<BaseResponse<Accounts>> GetAccountById(int id);
        Task<BaseResponse<bool>> DeleteAccount(int id);
        Task<BaseResponse<Accounts>> GetAccountByLogin(string login);
        Task<BaseResponse<Accounts>> Register(RegisterViewModel model);
        Task<BaseResponse<Accounts>> LogIn(LoginViewModel model);
        Task<BaseResponse<ClaimsIdentity>> Authentication(Accounts account);
        Task<BaseResponse<bool>> AccountActivation(string login);
        Task<BaseResponse<bool>> NewPassword(int id, string newPassword);
        Task<BaseResponse<bool>> SendEmail(string login, string message, string subject);
        Task<BaseResponse<int>> CreateConfirmationCode();
        Task<BaseResponse<bool>> ChangeRole(int accountId, UserRole selectedRole);
    }
}
