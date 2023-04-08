using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Enum;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace OnlineStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> _baseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsService> _logger;

        public OrderService(IBaseRepository<Order> baseRepository, IMapper mapper, ILogger<AccountsService> logger)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateOrder(Product product, Cart cart)
        {
            _logger.LogInformation("[CreateOrder]");

            try
            {
                var isOrderUnique = true;

                foreach (Order cartOrder in cart.Orders)
                {
                    if (cartOrder.Product != product)
                    {
                        continue;
                    }

                    isOrderUnique = false;

                    if (cartOrder.Amount + 1 > cartOrder.Product.Info.Amount)
                    {
                        return new BaseResponse<bool>()
                        {
                            Data = false,
                            StatusCode = StatusCode.OutOfRange,
                            Description = $"Only {cartOrder.Product.Info.Amount} units available"
                        };
                    }

                    cartOrder.Amount++;

                    await _baseRepository.Update(cartOrder);
                };

                if (isOrderUnique)
                {
                    Order order = new Order()
                    {
                        Product = product,
                        Cart = cart
                    };
                    await _baseRepository.Create(order);
                }

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateOrder]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> ChangeOrdersAmount(int id, int ordersAmount, bool isIncrease)
        {
            _logger.LogInformation("[DecreaseOrdersAmount]");

            try
            {
                var order = await _baseRepository.GetAll<Order>().FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Order not found"
                    };
                }

                order.Amount = isIncrease ? ++ordersAmount : --ordersAmount;
                await _baseRepository.Update(order);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DecreaseOrdersAmount]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteOrder(int id)
        {
            _logger.LogInformation("[DeleteOrder]");

            try
            {
                var order = await _baseRepository.GetAll<Order>().FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Order not found"
                    };
                }

                await _baseRepository.Delete(order);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteOrder]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<List<Order>>> GetOrders(int cartId)
        {
            _logger.LogInformation("[GetOrders]");

            try
            {
                var orders = await _baseRepository.GetAll<Order>().Where(o => o.CartId == cartId).ToListAsync();

                return new BaseResponse<List<Order>>()
                {
                    Data = orders,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetOrders]: {ex.Message}");

                return new BaseResponse<List<Order>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> ChangeOrdersAmountByInput(int id, int ordersAmount)
        {
            _logger.LogInformation("[ChangeOrdersAmountByInput]");

            try
            {
                var order = await _baseRepository.GetAll<Order>().FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Order not found"
                    };
                }

                order.Amount = ordersAmount;
                await _baseRepository.Update(order);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ChangeOrdersAmountByInput]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }
    }
}
