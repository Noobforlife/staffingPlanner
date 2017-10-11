using System;

namespace StaffingPlanner.Models
{
    public class AcademicYear
    {
        public Guid Id { get; set; }
        public virtual TermYear StartTerm { get; set; }
        public virtual TermYear EndTerm { get; set; }

    }

}