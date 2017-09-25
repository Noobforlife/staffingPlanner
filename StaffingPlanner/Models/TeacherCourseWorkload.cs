namespace StaffingPlanner.Models
{
	public class TeacherCourseWorkload
	{
		public CourseEdition Course { get; set; }
		public Teacher Teacher { get; set; }
		public int Workload { get; set; }
	}
}