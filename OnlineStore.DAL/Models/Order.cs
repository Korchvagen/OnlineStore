namespace OnlineStore.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }

        public int Amount { get; set; } = 1;
    }
}
