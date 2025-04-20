namespace PTManagementSystem.Presentation.DTOs
{
    public class PaymentInfoDto
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public string BillingAddress { get; set; }
    }
}