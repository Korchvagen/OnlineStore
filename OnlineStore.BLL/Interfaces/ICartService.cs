using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.ViewModels.Cart;

namespace OnlineStore.BLL.Interfaces
{
    public interface ICartService
    {
        Task<BaseResponse<bool>> CreateCart(Accounts account);
        Task<BaseResponse<Cart>> GetCart(int accountId);
        Task<BaseResponse<PaymentProofViewModel>> GetPaymentProofViewModel(int confirmationCode);
        Task<BaseResponse<bool>> ClearCart(int cartId);
    }
}
