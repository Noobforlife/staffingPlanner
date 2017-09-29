using System;
using System.Collections.Generic;
using StaffingPlanner.Models;
using System.Linq;
using StaffingPlanner.DAL;

namespace StaffingPlanner.ViewModels
{
	public class SimpleCourseViewModel
	{
		public Guid Id { get; set; }
        public TermYear TermYear { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public double Credits { get; set; }
		public Period Period { get; set; }
		public int AllocatedHours { get; set; }
		public int RemainingHours { get; set; }

    }

    public class DetailedCourseViewModel
    {
        public Guid Id { get; set; }
        public TermYear TermYear { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Credits { get; set; }
        public Period Period { get; set; }
        public int TotalHours { get; set; }
        public int AllocatedHours { get; set; }
        public int RemainingHours { get; set; }
        public int NumStudents { get; set; }
        public Teacher CourseResponsible { get; set; }
        public float HST { get; set; }
        public List<TeacherViewModel> Teachers { get; set; }
    }
}