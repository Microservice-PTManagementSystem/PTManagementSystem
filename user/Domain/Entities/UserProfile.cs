using System;

namespace PTManagementSystem.Domain.Entities
{
    public class UserProfile
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
        public Address Address { get; set; } = new Address();
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}