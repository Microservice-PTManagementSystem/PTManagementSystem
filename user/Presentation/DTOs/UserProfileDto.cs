namespace PTManagementSystem.Presentation.DTOs
{
     public class UserProfileDto
    {
        public PersonalInfoDto PersonalInfo { get;  set; }
        public ContactInfoDto ContactInfo { get;  set; }
        public AddressDto Address { get;  set; }

     
    }

    public class PersonalInfoDto
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public DateTime DateOfBirth { get;  set; }
        public string Gender { get;  set; }

    }

    public class ContactInfoDto
    {
        public string Email { get;  set; }
        public string Phone { get;  set; }

        
    }

    public class AddressDto
    {
        public string Street { get;  set; }
        public string City { get;  set; }
        public string State { get;  set; }
        public string Country { get;  set; }
        public string PostalCode { get;  set; }

    }
}