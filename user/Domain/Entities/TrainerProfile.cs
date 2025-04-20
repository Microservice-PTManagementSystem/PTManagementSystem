using System;
using System.Collections.Generic;

namespace PTManagementSystem.Domain.Entities
{
    public class TrainerProfile
    {
        public List<Specialization> Specializations { get;  set; }
        public List<Certification> Certifications { get;  set; }
        public List<TimeSlot> AvailableSlots { get;  set; }
        public int YearsOfExperience { get;  set; }
        public decimal HourlyRate { get;  set; }

      
    }
    public class Specialization
    {
        public string Value { get; set; }
    }
    public class Certification
    {
        public string Name { get;  set; }
        public string IssuingAuthority { get;  set; }
        public DateTime IssueDate { get;  set; }
        public DateTime? ExpiryDate { get;  set; }

    }

    public class TimeSlot
    {
        public DayOfWeek DayOfWeek { get;  set; }
        public TimeSpan StartTime { get;  set; }
        public TimeSpan EndTime { get;  set; }

      
    }
}