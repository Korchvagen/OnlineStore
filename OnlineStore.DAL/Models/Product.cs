using OnlineStore.DAL.Enum;

namespace OnlineStore.DAL.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductCategory Category { get; set; }

        public double Price { get; set; }

        public byte[] Image { get; set; }

        public string FileName { get; set; }

        public virtual ProductInfo? Info { get; set; }
    }
}
