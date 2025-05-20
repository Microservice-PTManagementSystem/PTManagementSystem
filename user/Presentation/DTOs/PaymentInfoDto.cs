namespace PTManagementSystem.Presentation.DTOs
{
    public class PaymentInfoDto
    {
        public string cardHolder { get; set; } = string.Empty;
        public string cardNumber { get; set; } = string.Empty;
        public string expiryMonth { get; set; } = string.Empty;
        public string expiryYear{ get; set; } = string.Empty;
        public string cvc { get; set; } = string.Empty;
        //public string BillingAddress { get; set; } = string.Empty;
    }
}