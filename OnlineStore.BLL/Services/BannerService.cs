using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.ViewModels.Banner;
using OnlineStore.DAL.Enum;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Models;
using OnlineStore.DAL.Response;

namespace OnlineStore.BLL.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBaseRepository<Banner> _baseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Banner> _logger;

        public BannerService(IBaseRepository<Banner> baseRepository, IMapper mapper, ILogger<Banner> logger)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateBanner(CreateBannerViewModel model)
        {
            _logger.LogInformation("[CreateBanner]");

            try
            {
                var bytes = ConvertImage(model.NewImage);

                if (bytes.StatusCode != StatusCode.OK)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "File conversion error",
                        StatusCode = StatusCode.FileConvertationError
                    };
                }

                Banner banner = new Banner()
                {
                    Link = model.Link,
                    Image = bytes.Data,
                    FileName = model.NewImage.FileName
                };
                await _baseRepository.Create(banner);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateBanner]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteBanner(int id)
        {
            _logger.LogInformation("[DeleteBanner]");

            try
            {
                var banner = await _baseRepository.GetAll<Banner>().FirstOrDefaultAsync(b => b.Id == id);

                if (banner == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data= false,
                        Description = "Banner not found",
                        StatusCode = StatusCode.BannerNotFound
                    };
                }

                await _baseRepository.Delete(banner);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteBanner]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateBanner(EditBannerViewModel model)
        {
            _logger.LogInformation("[UpdateBanner]");

            try
            {
                var banner = await _baseRepository.GetAll<Banner>().FirstOrDefaultAsync(b => b.Id == model.Id);

                if (banner == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Banner not found",
                        StatusCode = StatusCode.BannerNotFound
                    };
                }

                banner.Link = model.NewLink;

                if (model.NewImage != null)
                {
                    var bytes = ConvertImage(model.NewImage);

                    if (bytes.StatusCode != StatusCode.OK)
                    {
                        return new BaseResponse<bool>()
                        {
                            Description = "File conversion error",
                            StatusCode = StatusCode.FileConvertationError
                        };
                    }

                    banner.Image = bytes.Data;
                    banner.FileName = model.NewImage.FileName;
                }

                await _baseRepository.Update(banner);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UpdateBanner]: {ex.Message}");

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<BannerViewModel>>> GetBanners()
        {
            _logger.LogInformation("[GetBanners]");

            try {
                var banners = await _baseRepository.GetAll<Banner>().ToListAsync();

                if(banners.Count == 0)
                {
                    return new BaseResponse<IEnumerable<BannerViewModel>>()
                    {
                        Description = "Banners not found",
                        StatusCode =StatusCode.NoBanners
                    };
                }

                IEnumerable<BannerViewModel> bannerViewModelList = _mapper.Map<IEnumerable<Banner>, IEnumerable<BannerViewModel>>(banners);

                return new BaseResponse<IEnumerable<BannerViewModel>>()
                {
                    Data = bannerViewModelList,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[GetBanners]: {ex.Message}");

                return new BaseResponse<IEnumerable<BannerViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<BannerViewModel>> GetFirstBanner()
        {
            _logger.LogInformation("[GetFisrtBanner]");

            try
            {
                var banner = await _baseRepository.GetAll<Banner>().FirstAsync();

                if (banner == null)
                {
                    return new BaseResponse<BannerViewModel>()
                    {
                        Description = "Banner not found",
                        StatusCode = StatusCode.BannerNotFound
                    };
                }

                BannerViewModel bannerViewModel = _mapper.Map<BannerViewModel>(banner);

                return new BaseResponse<BannerViewModel>()
                {
                    Data = bannerViewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetFisrtBanner]: {ex.Message}");

                return new BaseResponse<BannerViewModel>()
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
