using AutoMapper;
using OnlineStore.BLL.ViewModels.Account;
using OnlineStore.BLL.ViewModels.Banner;
using OnlineStore.BLL.ViewModels.Products;
using OnlineStore.DAL.Models;

namespace OnlineStore.BLL.AutoMappers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() {
            CreateMap<Accounts, AccountViewModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
               .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<CreateProductViewModel, Product>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.Image, opt => opt.Ignore())
               .ForMember(dest => dest.FileName, opt => opt.Ignore());

            CreateMap<Product, CreateProductViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.PrevImage, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));

            CreateMap<CreateProductInfoViewModel, ProductInfo>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.LifeTime, opt => opt.MapFrom(src => src.LifeTime))
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Material == null ? "" : src.Material))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color == null ? "" : src.Color))
                .ForMember(dest => dest.Memory, opt => opt.MapFrom(src => src.Memory == null ? "" : src.Memory))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating == null ? 0 : double.Parse(src.Rating)))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<ProductInfo, CreateProductInfoViewModel>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.LifeTime, opt => opt.MapFrom(src => src.LifeTime))
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Material))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.Memory, opt => opt.MapFrom(src => src.Memory))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.ToString()))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<Banner, BannerViewModel>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForPath(parent => parent.EditBannerViewModel.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(parent => parent.EditBannerViewModel.NewLink, opt => opt.MapFrom(src => src.Link));
        }
    }
}
