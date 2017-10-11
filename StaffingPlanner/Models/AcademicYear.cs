using System;

namespace StaffingPlanner.Models
{
    public class AcademicYear
    {
        public Guid Id { get; set; }
        public TermYear StartTerm { get; set; }
        public TermYear EndTerm { get; set; }

    }

}