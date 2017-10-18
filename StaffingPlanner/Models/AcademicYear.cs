using StaffingPlanner.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace StaffingPlanner.Models
{
    public class AcademicYear
    {
        public Guid Id { get; set; }
        public virtual TermYear StartTerm { get; set; }
        public virtual TermYear EndTerm { get; set; }
    }
}