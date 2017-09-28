using System;
using System.Collections.Generic;
using System.Linq;
using StaffingPlanner.DAL;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

namespace StaffingPlanner.Models
{
	public class CourseOffering
	{
		public Guid Id { get; set; }
        public virtual Course Course { get; set; }
        public string Term { get; set; }
		public Credits Credits { get; set; }
		public List<Period> Periods { get; set; }
		public int Budget { get; set; }
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