using System;
using System.Collections.Generic;
using StaffingPlanner.Models;

namespace StaffingPlanner.ViewModels
{
	public class SimpleCourseViewModel
	{
		public Guid Id { get; set; }
        public TermYear TermYear { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public double Credits { get; set; }
		public string Period { get; set; }
        public Teacher CourseResponsible { get; set; }
		public int AllocatedHours { get; set; }
		public int RemainingHours { get; set; }
        public CourseState State { get; set; }
        public string Status { get; set; }
    }

    public class DetailedCourseViewModel
    {
        public Guid Id { get; set; }
        public TermYear TermYear { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Credits { get; set; }
        public string Period { get; set; }
        public int TotalHours { get; set; }
        public int AllocatedHours { get; set; }
        public int RemainingHours { get; set; }
        public int NumStudents { get; set; }
        public int RegisteredStudents { get; set; }
        public int PassedStudents { get; set; }
        public Teacher CourseResponsible { get; set; }
        public float HST { get; set; }
        public List<SimpleTeacherViewModel> Teachers { get; set; }
        public string Status { get; set; }
    }

    public class CourseTeacherViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AcademicTitle Title { get; set; }
        public int AllocatedFall { get; set; }
        public int AllocatedSpring { get; set; }
        public int RemainingTeachingHours { get; set; }
        public int AllocatedCourse { get; set; }
    }
}