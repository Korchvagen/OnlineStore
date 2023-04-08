namespace OnlineStore.DAL.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string FileName { get; set; }
        public string Link { get; set; }
    }
}
