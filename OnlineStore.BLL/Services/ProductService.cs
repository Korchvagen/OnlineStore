using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Enum;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Response;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Products;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace OnlineStore.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _baseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Product> _logger;

        public ProductService(IBaseRepository<Product> baseRepository, IMapper mapper, ILogger<Product> logger)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<Product>>> GetFilteredProducts(FilterViewModel model)
        {
            _logger.LogInformation("[GetFilteredProducts]");

            try
            {
                BaseResponse<IEnumerable<Product>> filteredProducts;
                var products = await _baseRepository.GetAll<Product>().ToListAsync();

                if (products.Count == 0)
                {
                    return new BaseResponse<IEnumerable<Product>>()
                    {
                        Description = "Found 0 items",
                        StatusCode = StatusCode.NoProducts
                    };
                }
                else
                {
                    filteredProducts = FilterProducts(products, model);
                }

                if (filteredProducts.Data == null)
                {
                    return new BaseResponse<IEnumerable<Product>>()
                    {
                        Description = "No such products",
                        StatusCode = StatusCode.NoSuchProducts
                    };
                }

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Data = filteredProducts.Data,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetFilteredProducts]: {ex.Message}");

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Product>> GetProductById(int id)
        {
            _logger.LogInformation("[GetAccountById]");

            try
            {
                var product = await _baseRepository.GetAll<Product>().FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return new BaseResponse<Product>()
                    {
                        Description = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                return new BaseResponse<Product>()
                {
                    Data = product,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetAccountById]: {ex.Message}");

                return new BaseResponse<Product>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("[GetProducts]");

            try
            {
                var products = await _baseRepository.GetAll<Product>().ToListAsync();

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
                _logger.LogError(ex, $"[GetProducts]: {ex.Message}");

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<CreateProductViewModel>> CreateProduct(CreateProductViewModel model)
        {
            _logger.LogInformation("[CreateProduct]");

            try
            {
                var bytes = ConvertImage(model.Image);

                if (bytes.StatusCode != StatusCode.OK)
                {
                    return new BaseResponse<CreateProductViewModel>()
                    {
                        Description = "File conversion error",
                        StatusCode = StatusCode.FileConvertationError
                    };
                }

                Product product = _mapper.Map<Product>(model);

                product.Image = bytes.Data;
                product.FileName = model.Image.FileName;
                await _baseRepository.Create(product);

                CreateProductViewModel createproductViewModel = _mapper.Map<CreateProductViewModel>(product);

                return new BaseResponse<CreateProductViewModel>()
                {
                    Data = createproductViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateProduct]: {ex.Message}");

                return new BaseResponse<CreateProductViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<CreateProductViewModel>> GetProductFormData(int id)
        {
            _logger.LogInformation("[GetProductFormData]");

            try
            {
                var product = await _baseRepository.GetAll<Product>().FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return new BaseResponse<CreateProductViewModel>()
                    {
                        Description = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                CreateProductViewModel createproductViewModel = _mapper.Map<CreateProductViewModel>(product);

                return new BaseResponse<CreateProductViewModel>()
                {
                    Data = createproductViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetProductFormData]: {ex.Message}");

                return new BaseResponse<CreateProductViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<CreateProductViewModel>> UpdateProductData(CreateProductViewModel model)
        {
            _logger.LogInformation("[UpdateProductData]");

            try
            {
                var product = await _baseRepository.GetAll<Product>().FirstOrDefaultAsync(p => p.Id == model.Id);

                if (product == null)
                {
                    return new BaseResponse<CreateProductViewModel>()
                    {
                        Description = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                _mapper.Map<CreateProductViewModel, Product>(model, product);

                if (model.Image != null)
                {
                    var bytes = ConvertImage(model.Image);

                    if (bytes.StatusCode != StatusCode.OK)
                    {
                        return new BaseResponse<CreateProductViewModel>()
                        {
                            Description = "File conversion error",
                            StatusCode = StatusCode.FileConvertationError
                        };
                    }

                    product.Image = bytes.Data;
                    product.FileName = model.Image.FileName;
                }

                await _baseRepository.Update(product);

                CreateProductViewModel createproductViewModel = _mapper.Map<CreateProductViewModel>(product);

                return new BaseResponse<CreateProductViewModel>()
                {
                    Data = createproductViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UpdateProductData]: {ex.Message}");

                return new BaseResponse<CreateProductViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteProduct(int id)
        {
            _logger.LogInformation("[DeleteProduct]");

            try
            {
                var product = await _baseRepository.GetAll<Product>().FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                await _baseRepository.Delete(product);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteProduct]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Product>> GetLastSliderProduct(bool isFromStart)
        {
            _logger.LogInformation("[GetHighestRatedProduct]");

            try
            {
                var products = _baseRepository.GetAll<Product>().Where(p => p.Info.Rating >= 7).OrderByDescending(p => p.Info.Rating);
                var product = isFromStart
                    ? await products.FirstAsync()
                    : await products.LastOrDefaultAsync();

                if (product == null)
                {
                    return new BaseResponse<Product>()
                    {
                        Description = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                return new BaseResponse<Product>()
                {
                    Data = product,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetHighestRatedProduct]: {ex.Message}");

                return new BaseResponse<Product>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Product>> GetSliderProduct(int productId, bool isFromStart, int cardsNumber)
        {
            _logger.LogInformation("[GetSliderProduct]");

            try
            {
                var currentProduct = await _baseRepository.GetAll<Product>().FirstOrDefaultAsync(p => p.Id == productId);
                var products = _baseRepository.GetAll<Product>().Where(p => p.Info.Rating >= 7 && p.Id != currentProduct.Id).OrderByDescending(p => p.Info.Rating);

                if (products == null)
                {
                    return new BaseResponse<Product>()
                    {
                        Description = "Product not found",
                        StatusCode = StatusCode.ProductNotFound
                    };
                }

                if (products.ToList().Count < cardsNumber)
                {
                    return new BaseResponse<Product>()
                    {
                        Description = "Out of products range",
                        StatusCode = StatusCode.LastProduct
                    };
                }

                var product = isFromStart
                ? await products.FirstOrDefaultAsync(p => p.Info.Rating <= currentProduct.Info.Rating)
                : await products.LastOrDefaultAsync(p => p.Info.Rating >= currentProduct.Info.Rating);

                return new BaseResponse<Product>()
                {
                    Data = product,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetSliderProduct]: {ex.Message}");

                return new BaseResponse<Product>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private BaseResponse<IEnumerable<Product>> FilterProducts(List<Product> products, FilterViewModel model)
        {
            _logger.LogInformation("[ProductsFiltration]");

            try
            {
                if(model.Price != null)
                {
                    products = products.Where(product => product.Price <= double.Parse(model.Price)).ToList();
                }

                if (model.Category != ProductCategory.All)
                {
                    products = products.Where(product => product.Category == model.Category).ToList();
                }

                if (model.Name != null)
                {
                    products = products.Where(product => product.Name.ToLower().Contains(model.Name.ToLower())).ToList();
                }

                if(model.Availability != false)
                {
                    products = products.Where(product => product.Info!.Amount != 0).ToList();
                }

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Data = products,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ProductsFiltration]: {ex.Message}");

                return new BaseResponse<IEnumerable<Product>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private BaseResponse<byte[]> ConvertImage(IFormFile image)
        {
            _logger.LogInformation("[ConvertImage]");

            try
            {
                MemoryStream memoryStream = new MemoryStream();
                image.CopyTo(memoryStream);

                return new BaseResponse<byte[]>()
                {
                    Data = memoryStream.ToArray(),
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ConvertImage]: {ex.Message}");

                return new BaseResponse<byte[]>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
