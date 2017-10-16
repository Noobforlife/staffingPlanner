using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StaffingPlanner.Models
{
    public class TeacherTaskShare
    {
        public Guid Id { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Teacher Teacher { get; set; }
        public decimal TeachingShare { get; set; }
        public decimal ResearchShare { get; set; }
        public decimal AdminShare { get; set; }
        public decimal OtherShare { get; set; }
        public string Comment { get; set; }

    }
}