using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;

namespace OnlineStore.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<bool>> CreateOrder(Product product, Cart cart);
        Task<BaseResponse<bool>> DeleteOrder(int id);
        Task<BaseResponse<List<Order>>> GetOrders(int cartId);
        Task<BaseResponse<bool>> ChangeOrdersAmount(int id, int ordersAmount, bool isIncrease);
        Task<BaseResponse<bool>> ChangeOrdersAmountByInput(int id, int ordersAmount);
    }
}
