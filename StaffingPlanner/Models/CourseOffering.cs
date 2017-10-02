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
        public virtual TermYear TermYear { get; set; }
		public double Credits { get; set; }
		public Period Periods { get; set; }
		public int TotalHours { get; set; }
		public virtual ICollection<Teacher> Teachers { get; set; }
		public virtual Teacher CourseResponsible { get; set; }
		public float HST { get; set; }
		public int NumStudents { get; set; }
		public string Status => CourseController.GetStatus();
		public int RemainingHours => TotalHours - AllocatedHours;
		public int AllocatedHours
		{
			get
			{
				var db = StaffingPlanContext.GetContext();
				return db.Workloads
					.Where(w => w.Course.Id.Equals(Id))
					.Sum(w => w.Workload);
			}
		}

		public override bool Equals(object obj)
		{
			var ce = obj as CourseOffering;
			return ce != null && ce.Id == Id;
		}
	}
}

#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()