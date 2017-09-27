using System;
using System.Collections.Generic;
using StaffingPlanner.Models;

namespace StaffingPlanner.ViewModels
{
	public class SimpleCourseViewModel
	{
		public Guid Id { get; set; }
        public string Term { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public Credits Credits { get; set; }
		public List<Period> Periods { get; set; }
		public int AllocatedHours { get; set; }
		public int RemainingHours { get; set; }
	}

    public class DetailedCourseViewModel
    {
        public Guid Id { get; set; }
        public string Term { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Credits Credits { get; set; }
        public List<Period> Periods { get; set; }
        public int TotalHours { get; set; }
        public int AllocatedHours { get; set; }
        public int RemainingHours { get; set; }
        public int NumStudents { get; set; }
        public Teacher CourseResponsible { get; set; }
        public float HST { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}