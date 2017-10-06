using System;

namespace StaffingPlanner.Models
{
    //The assigned hours a teacher has for a given course offering
	public class TeacherCourseWorkload
	{
        public Guid Id { get; set; }
		public virtual CourseOffering Course { get; set; }
		public virtual Teacher Teacher { get; set; }
		public int Workload { get; set; }
	}
}