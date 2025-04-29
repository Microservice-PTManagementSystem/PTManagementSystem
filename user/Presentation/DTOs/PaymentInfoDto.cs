namespace PTManagementSystem.Presentation.DTOs
{
    public class PaymentInfoDto
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvv { get; set; }
        public string BillingAddress { get; set; }

    }
}