namespace OnlineStore.BLL.ViewModels.Cart
{
    public class PaymentProofViewModel
    {
        public int ConfirmationCode { get; set; }

        public int Code { get; set; }

        public DateTime ConfirmationTime { get; set; }

        public DateTime Time { get; set; }
    }
}
