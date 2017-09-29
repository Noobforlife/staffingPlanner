using System;
using StaffingPlanner.Models;
using System.Collections.Generic;

namespace StaffingPlanner.ViewModels
{

	public class TeacherViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
        public string Email { get; set; }
		public AcademicTitle Title { get; set; }
		public int TotalHours { get; set; }
		public int FallWork { get; set; }
		public int SpringWork { get; set; }
		public int RemainingHours { get; set; }
        public List<TeacherCourseViewModel> Courses { get; set; }
	}
}