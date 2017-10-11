using System;
using System.Collections.Generic;
using System.Linq;
using StaffingPlanner.Controllers;
using StaffingPlanner.DAL;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

namespace StaffingPlanner.Models
{
    public class CourseOffering
    {
        public Guid Id { get; set; }
        public virtual Course Course { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual TermYear TermYear { get; set; }
		public double Credits { get; set; }
        public CourseState State { get; set; }
		public Period Periods { get; set; }
		public int TotalHours { get; set; }
		public virtual ICollection<Teacher> Teachers { get; set; }
		public virtual Teacher CourseResponsible { get; set; }
		public float HST { get; set; }
		public int NumStudents { get; set; }
        public int RegisteredStudents { get; set; }
        public int PassedStudents { get; set; }
        public int RemainingHours => TotalHours - AllocatedHours;
		public int AllocatedHours
		{
			get
			{
				var db = StaffingPlanContext.GetContext();
                var workloads = db.Workloads.Where(w => w.Course.Id.Equals(Id)).ToList();
                if (workloads.Count == 0) { return 0; }
                return workloads.Select(x => x.Workload).Sum();
			}
		}
        public string Status => CourseController.GetStatus(TotalHours,AllocatedHours);

        public override bool Equals(object obj)
		{
			var ce = obj as CourseOffering;
			return ce != null && ce.Id == Id;
		}
	}
}

#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()