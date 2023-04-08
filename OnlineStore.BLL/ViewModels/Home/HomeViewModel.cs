using OnlineStore.BLL.ViewModels.Banner;
using OnlineStore.DAL.Models;

namespace OnlineStore.BLL.ViewModels.Home
{
    public class HomeViewModel
    {
        public Accounts? Account { get; set; }

        public Product Product { get; set; }

        public BannerViewModel Banner { get; set; }
    }
}
