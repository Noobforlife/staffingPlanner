using StaffingPlanner.Models;

namespace StaffingPlanner.ViewModels
{
	public class TeacherViewModel
	{
		public string Name { get; set; }
		public AcademicTitle Title { get; set; }
		public int TotalHours { get; set; }
		public int FallWork { get; set; }
		public int SpringWork { get; set; }
		public int RemainingHours { get; set; }
	}
}