using System;
using System.Collections.Generic;

namespace PTManagementSystem.Presentation.DTOs
{
    public class TrainerProfileDto
    {
        public string Specialization { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public List<CertificationDto> Certifications { get; set; } = new List<CertificationDto>();
        public string Bio { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }
        public List<string> AvailableDays { get; set; } = new List<string>();
        public List<TimeSpan> AvailableHours { get; set; } = new List<TimeSpan>();
    }

    public class CertificationDto
    {
        public string Name { get; set; } = string.Empty;
        public string IssuingAuthority { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}