using System;
using StaffingPlanner.Models;

namespace StaffingPlanner.ViewModels
{

	public class TeacherViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public AcademicTitle Title { get; set; }
		public int TotalHours { get; set; }
		public EmploymentLevel FallWork { get; set; }
		public EmploymentLevel SpringWork { get; set; }
		public int RemainingHours { get; set; }
	}
}