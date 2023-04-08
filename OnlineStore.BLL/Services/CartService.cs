using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Enum;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Cart;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace OnlineStore.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseRepository<Cart> _baseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(IBaseRepository<Cart> baseRepository, IMapper mapper, ILogger<CartService> logger)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateCart(Accounts account)
        {
            _logger.LogInformation("[CreateCart]");

            try
            {
                Cart cart = new Cart()
                {
                    Account = account
                };
                await _baseRepository.Create(cart);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[CreateCart]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<Cart>> GetCart(int accountId)
        {
            _logger.LogInformation("[GetCart]");

            try
            {
                var cart = await _baseRepository.GetAll<Cart>().FirstOrDefaultAsync(c => c.AccountsId == accountId);

                if(cart == null)
                {
                    return new BaseResponse<Cart>()
                    {
                        StatusCode = StatusCode.CartNotFound,
                        Description = "Cart not found"
                    };
                }
                return new BaseResponse<Cart>()
                {
                    Data = cart,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[GetCart]: {ex.Message}");

                return new BaseResponse<Cart>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<PaymentProofViewModel>> GetPaymentProofViewModel(int confirmationCode)
        {
            _logger.LogInformation("[GetPaymentProofViewModel]");

            try
            {
                PaymentProofViewModel paymentProofViewModel = new PaymentProofViewModel()
                {
                    ConfirmationCode = confirmationCode,
                    ConfirmationTime = DateTime.Now
                };

                return new BaseResponse<PaymentProofViewModel>()
                {
                    Data = paymentProofViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[GetPaymentProofViewModel]: {ex.Message}");

                return new BaseResponse<PaymentProofViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> ClearCart(int cartId)
        {
            _logger.LogInformation("[DeleteOrders]");

            try
            {
                var cart = await _baseRepository.GetAll<Cart>().FirstOrDefaultAsync(c => c.Id == cartId);

                if(cart == null) 
                { 
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.CartNotFound,
                        Description = "Cart not found"
                    };
                }

                if(cart.Orders?.Count == 0)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NoOrders,
                        Description = "No orders"
                    };
                }

                cart.Orders?.Clear();
                await _baseRepository.Update(cart);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteOrders]: {ex.Message}");

                return new BaseResponse<bool> ()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }
    }
}
