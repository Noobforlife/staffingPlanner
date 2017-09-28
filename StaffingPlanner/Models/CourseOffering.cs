using System;
using System.Collections.Generic;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

namespace StaffingPlanner.Models
{
    public class CourseOffering
	{
		public Guid Id { get; set; }
        public virtual Course Course { get; set; }
        public TermYear TermYear { get; set; }
		public Credits Credits { get; set; }
		public Period Periods { get; set; }
		public int TotalHours { get; set; }
		public virtual ICollection<Teacher> Teachers { get; set; }
		public virtual Teacher CourseResponsible { get; set; }
		public float HST { get; set; }
		public int NumStudents { get; set; }
      
		public override bool Equals(object obj)
		{
			var ce = obj as CourseOffering;
			return ce != null && ce.Id == Id;
		}
	}
}

#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()