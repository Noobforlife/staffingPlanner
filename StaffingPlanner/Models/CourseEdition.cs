using System;
using System.Collections.Generic;
using System.Linq;
using StaffingPlanner.DAL;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

namespace StaffingPlanner.Models
{
	public class CourseEdition
	{
		public Guid Id { get; set; }
		public string SchoolYear { get; set; }
		public Credits Credits { get; set; }
		public Term Term { get; set; }
		public Period Period { get; set; }
		public int Budget { get; set; }
		public ICollection<Teacher> Teachers { get; set; }
		public Teacher CourseResponsible { get; set; }

		public int GetAllocatedHours()
		{
			var db = StaffingPlanContext.GetContext();
			return db.Workloads.Where(w => w.Course.Equals(this)).Select(w => w.Workload).Sum();
		}

		public int GetRemainingHours()
		{
			return Budget - GetAllocatedHours();
		}

		public override bool Equals(object obj)
		{
			var ce = obj as CourseEdition;
			return ce != null && ce.Id == Id;
		}
	}
}

#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()