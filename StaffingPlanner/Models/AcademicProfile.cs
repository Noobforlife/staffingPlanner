using System;
using System.ComponentModel.DataAnnotations;

namespace StaffingPlanner.Models
{
    public enum AcademicTitle
    {
        Professor,
        Lektor,
        Adjunkt,
        Doktorand,
        Amanuens,
        Assistent
    }

    public class AcademicProfile
    {
        [Key]
        public AcademicTitle Title { get; set; }
        public decimal TeachingShare { get; set; }
        public decimal ResearchShare { get; set; }
        public decimal AdminShare { get; set; }
        public decimal OtherShare { get; set; }

    }
}