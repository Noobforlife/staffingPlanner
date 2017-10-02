using System;

namespace StaffingPlanner.Models
{
	public class TeacherCourseWorkload
	{
        public Guid Id { get; set; }
		public virtual CourseOffering Course { get; set; }
		public virtual Teacher Teacher { get; set; }
		public int Workload { get; set; }
	}
}