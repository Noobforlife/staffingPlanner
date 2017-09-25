using System;
using System.Collections.Generic;
using System.Linq;

namespace StaffingPlanner.Models
{
	public class Course
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public ICollection<CourseEdition> Editions { get; set; }

		public CourseEdition GetEdition(string schoolYear)
		{
			return Editions.First(e => e.SchoolYear == schoolYear);
		}
	}
}