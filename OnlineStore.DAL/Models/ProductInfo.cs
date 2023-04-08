using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DAL.Models
{
    public class ProductInfo
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public int LifeTime { get; set; }

        public string Material { get; set; }

        public string Color { get; set; }

        public string Memory { get; set; }

        public double Rating { get; set; }

        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }
    }
}
