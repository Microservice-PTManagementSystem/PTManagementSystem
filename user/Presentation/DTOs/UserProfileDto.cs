namespace PTManagementSystem.Presentation.DTOs
{
    public class UserProfileDto
    {
        //public string FirstName { get; set; } = string.Empty;
        //public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public double Height { get; set; }
        public double Weight { get; set; }
        public string FitnessGoals { get; set; } = string.Empty;
        public string MedicalConditions { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new AddressDto();
    }

    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}