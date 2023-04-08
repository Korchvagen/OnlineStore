using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Enum;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Products;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace OnlineStore.BLL.Services
{
    public class ProductInfoService : IProductInfoService
    {
        private readonly IBaseRepository<ProductInfo> _baseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductInfo> _logger;

        public ProductInfoService(IBaseRepository<ProductInfo> baseRepository, IMapper mapper, ILogger<ProductInfo> logger)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CreateProductInfoViewModel>> CreateProductInfo(CreateProductInfoViewModel model)
        {
            _logger.LogInformation("[CreateProductInfo]");

            try
            {
                ProductInfo productInfo = _mapper.Map<ProductInfo>(model);
                await _baseRepository.Create(productInfo);

                CreateProductInfoViewModel createProductInfoViewModel = _mapper.Map<CreateProductInfoViewModel>(productInfo);

                return new BaseResponse<CreateProductInfoViewModel>()
                {
                    Data = createProductInfoViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateProduct]: {ex.Message}");

                return new BaseResponse<CreateProductInfoViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ProductInfo>> GetProductInfo(int id)
        {
            _logger.LogInformation("[GetProductInfo]");

            try
            {
                var productInfo = await _baseRepository.GetAll<ProductInfo>().FirstOrDefaultAsync(p => p.ProductId == id);

                if (productInfo == null)
                {
                    return new BaseResponse<ProductInfo>()
                    {
                        Description = "Product information not found",
                        StatusCode = StatusCode.ProductInfoNotFound
                    };
                }

                return new BaseResponse<ProductInfo>()
                {
                    Data = productInfo,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetProductInfo]: {ex.Message}");

                return new BaseResponse<ProductInfo>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<CreateProductInfoViewModel>> GetProductInfoFormData(int? id)
        {
            _logger.LogInformation("[GetProductInfoFormData]");

            try
            {
                var productInfo = await _baseRepository.GetAll<ProductInfo>().FirstOrDefaultAsync(p => p.ProductId == id);

                if (productInfo == null)
                {
                    return new BaseResponse<CreateProductInfoViewModel>()
                    {
                        Description = "Product information not found",
                        StatusCode = StatusCode.ProductInfoNotFound
                    };
                }

                CreateProductInfoViewModel createProductInfoViewModel = _mapper.Map<CreateProductInfoViewModel>(productInfo);

                return new BaseResponse<CreateProductInfoViewModel>()
                {
                    Data = createProductInfoViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetProductInfoFormData]: {ex.Message}");

                return new BaseResponse<CreateProductInfoViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("[GetProductsFromInfo]");

            try
            {
                var products = await _baseRepository.GetAll<ProductInfo>().Select(p => p.Product).ToListAsync();

                if (products == null)
                {
                    return new BaseResponse<IEnumerable<Product>>()
                    {
                        Description = "Found 0 items",
                        StatusCode = StatusCode.NoProducts
                    };
                }

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Data = products,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetProductsFromInfo]: {ex.Message}");

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> ReduceAmount(List<Order> orders)
        {
            _logger.LogInformation("[ReduceAmount]");

            try
            {
                if (orders.Count == 0)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCode.NoOrders,
                        Description = "No orders"
                    };
                }

                foreach (Order order in orders)
                {
                    order.Product.Info.Amount = order.Product.Info.Amount - order.Amount;
                    await _baseRepository.Update(order.Product.Info);
                }

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReduceAmount]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> RememberProductInfoData(CreateProductInfoViewModel model)
        {
            _logger.LogInformation("[RememberProductInfoData]");

            try
            {
                var productInfo = await _baseRepository.GetAll<ProductInfo>().FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

                if (productInfo == null)
                {
                    ProductInfo product = _mapper.Map<ProductInfo>(model);
                    await _baseRepository.Create(product);

                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        StatusCode = StatusCode.OK
                    };
                }

                _mapper.Map<CreateProductInfoViewModel, ProductInfo>(model, productInfo);
                await _baseRepository.Update(productInfo);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[RememberProductInfoData]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<CreateProductInfoViewModel>> UpdateProductInfoData(CreateProductInfoViewModel model)
        {
            _logger.LogInformation("[UpdateProductInfoData]");

            try
            {
                var productInfo = await _baseRepository.GetAll<ProductInfo>().FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

                if (productInfo == null)
                {
                    return new BaseResponse<CreateProductInfoViewModel>()
                    {
                        Description = "Product information not found",
                        StatusCode = StatusCode.ProductInfoNotFound
                    };
                }

                _mapper.Map<CreateProductInfoViewModel, ProductInfo>(model, productInfo);
                await _baseRepository.Update(productInfo);

                CreateProductInfoViewModel createProductInfoViewModel = _mapper.Map<CreateProductInfoViewModel>(productInfo);

                return new BaseResponse<CreateProductInfoViewModel>()
                {
                    Data = createProductInfoViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UpdateProductInfoData]: {ex.Message}");

                return new BaseResponse<CreateProductInfoViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
