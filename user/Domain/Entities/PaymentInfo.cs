namespace PTManagementSystem.Domain.Entities
{
	public class PaymentInfo
	{
		public string CardHolderName { get;  set; }
		public string CardNumber { get;  set; }
		public string ExpirationMonth { get;  set; }
		public string ExpirationYear { get;  set; }
		public string Cvv { get;  set; }
		public string BillingAddress { get; set; }

	}
}