namespace PTManagementSystem.Presentation.DTOs
{
    public class PaymentInfoDto
    {
        public string CardHolderName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
    }
}