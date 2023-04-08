namespace OnlineStore.DAL.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int AccountsId { get; set; }

        public virtual Accounts Account { get; set; }

        public virtual List<Order>? Orders { get; set; }
    }
}
