using System;

namespace StaffingPlanner.Models
{
	public class TeacherCourseWorkload
	{
		public Guid Id { get; set; }
		public CourseOffering Course { get; set; }
		public Teacher Teacher { get; set; }
		public int Workload { get; set; }
	}
}