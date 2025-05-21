namespace PTManagementSystem.Domain.Entities
{
	public class PaymentInfo
	{
		public string CardHolderName { get; set; } = string.Empty;
		public string CardNumber { get; set; } = string.Empty;
		public string ExpiryMonth { get; set; } = string.Empty;
		public string ExpiryYear { get; set; } = string.Empty;
		public string CVC { get; set; } = string.Empty;
		//public string BillingAddress { get; set; } = string.Empty;
	}
}