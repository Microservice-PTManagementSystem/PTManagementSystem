using System;

namespace PTManagementSystem.Domain.Entities
{
    public class UserProfile
    {
        public PersonalInfo PersonalInfo { get;  set; }
        public ContactInfo ContactInfo { get;  set; }
        public Address Address { get;  set; }

     
    }

    public class PersonalInfo
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public DateTime DateOfBirth { get;  set; }
        public string Gender { get;  set; }

    }

    public class ContactInfo
    {
        public string Email { get;  set; }
        public string Phone { get;  set; }

        
    }

    public class Address
    {
        public string Street { get;  set; }
        public string City { get;  set; }
        public string State { get;  set; }
        public string Country { get;  set; }
        public string PostalCode { get;  set; }

    }
}