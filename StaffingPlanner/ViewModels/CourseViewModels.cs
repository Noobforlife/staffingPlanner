using System;
using StaffingPlanner.Models;

namespace StaffingPlanner.ViewModels
{
	public class CourseViewModels
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public Credits Credits { get; set; }
		public Term Term { get; set; }
		public Period Period { get; set; }
		public int AllocatedHours { get; set; }
		public int RemainingHours { get; set; }
	}
}