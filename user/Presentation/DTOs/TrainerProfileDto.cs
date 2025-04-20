namespace PTManagementSystem.Presentation.DTOs
{
     public class TrainerProfileDto
    {
        public List<string> Specializations { get;  set; }
        public List<CertificationDto> Certifications { get;  set; }
        public List<TimeSlotDto> AvailableSlots { get;  set; }
        public int YearsOfExperience { get;  set; }
        public decimal HourlyRate { get;  set; }

      
    }

    public class CertificationDto
    {
        public string Name { get;  set; }
        public string IssuingAuthority { get;  set; }
        public DateTime IssueDate { get;  set; }
        public DateTime? ExpiryDate { get;  set; }

    }

    public class TimeSlotDto
    {
        public DayOfWeek DayOfWeek { get;  set; }
        public TimeSpan StartTime { get;  set; }
        public TimeSpan EndTime { get;  set; }

      
    }
}