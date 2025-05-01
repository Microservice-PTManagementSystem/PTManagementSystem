using System;
using System.Collections.Generic;

namespace PTManagementSystem.Domain.Entities
{
    public class TrainerProfile
    {
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public List<Certification> Certifications { get; set; } = new List<Certification>();
        public string Bio { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public List<string> AvailableDays { get; set; } = new List<string>();
        public List<TimeSpan> AvailableHours { get; set; } = new List<TimeSpan>();
    }

    public class Certification
    {
        public string Name { get; set; } = string.Empty;
        public string IssuingAuthority { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}