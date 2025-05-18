namespace PTManagementSystem.Domain.Entities
{
	public class PaymentInfo
	{
		public string CardHolderName { get; set; } = string.Empty;
		public string CardNumber { get; set; } = string.Empty;
		public string ExpiryDay { get; set; } = string.Empty;
		public string ExpiryYear { get; set; } = string.Empty;
		public string CVV { get; set; } = string.Empty;
		public string BillingAddress { get; set; } = string.Empty;
	}
}