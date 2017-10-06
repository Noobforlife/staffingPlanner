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
		public int FallAvailability { get; set; }
		public int SpringAvailability { get; set; }
		public int RemainingHours { get; set; }
        public string Status { get; set; }
        public List<SimpleCourseViewModel> Courses { get; set; }
	}

    //Not currently used as the courses in teacher details use SimpleCourseViewModel
    //When we want to present different information from the course list we will need this
    //
    //public class TeacherCourseViewModel
    //{
    //    public string CourseName { get; set; }
    //    public string CourseCode { get; set; }
    //    public string CourseInfo { get; set; }
    //    public int CourseAllocatedHours { get; set; }
    //    public int CourseRemainingHours { get; set; }
    //    public int TeacherAssignedHours { get; set; }
    //}
}