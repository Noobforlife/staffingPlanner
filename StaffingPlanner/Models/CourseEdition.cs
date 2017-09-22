using System;
using System.Collections.Generic;

namespace StaffingPlanner.Models
{
	public class CourseEdition
	{
		public Guid Id { get; set; }
		public string SchoolYear { get; set; }
		public Term Term { get; set; }
		public Period Period { get; set; }
		public int Budget { get; set; }
		public ICollection<Teacher> Teachers { get; set; }
		public Teacher CourseResponsible { get; set; }
	}
}