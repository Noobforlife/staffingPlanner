using System;
using System.Collections.Generic;

namespace StaffingPlanner.Models
{
	public class Course
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public ICollection<CourseEdition> Editions { get; set; }
	}
}