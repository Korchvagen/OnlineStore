using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Repositories;
using OnlineStore.DAL.Models;
using OnlineStore.BLL.Services;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.AutoMappers;

namespace OnlineStore
{
    public static class ProgramInit
    {
        public static void RepositoriesInitializer(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Accounts>, BaseRepository<Accounts>>();
            services.AddScoped<IBaseRepository<Product>, BaseRepository<Product>>();
            services.AddScoped<IBaseRepository<ProductInfo>, BaseRepository<ProductInfo>>();
            services.AddScoped<IBaseRepository<Cart>, BaseRepository<Cart>>();
            services.AddScoped<IBaseRepository<Order>, BaseRepository<Order>>();
            services.AddScoped<IBaseRepository<Banner>, BaseRepository<Banner>>();
        }

        public static void ServicesInitializer(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductInfoService, ProductInfoService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddAutoMapper(typeof(AppMappingProfile));
        }
    }
}
