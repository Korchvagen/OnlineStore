namespace OnlineStore.BLL.ViewModels.Banner
{
    public class BannerViewModel
    {
        public byte[] Image { get; set; }

        public string FileName { get; set; }

        public string Link { get; set; }

        public EditBannerViewModel EditBannerViewModel { get; set; }
    }
}
