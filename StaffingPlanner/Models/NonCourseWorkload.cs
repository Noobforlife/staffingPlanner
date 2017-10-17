using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffingPlanner.Models
{
    public class NonCourseWorkload
    {
        public Guid Id { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual TermYear TermYear { get; set; }
        public int Workload { get; set; }
    }
}