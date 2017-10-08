using System;
using StaffingPlanner.Models;
using System.Collections.Generic;

namespace StaffingPlanner.ViewModels
{

	public class SimpleTeacherViewModel
	{
		public Guid Id { get; set; }
        public string Name { get; set; }
		public AcademicTitle Title { get; set; }
        public int FallTermAvailability { get; set; }
        public int SpringTermAvailability { get; set; }
		public int AllocatedHoursFall { get; set; }
		public string StatusFall { get; set; }
        public int AllocatedHoursSpring { get; set; }
		public string StatusSpring { get; set; }
        public int TotalRemainingHours { get; set; }
        
	}

    public class DetailedTeacherViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AcademicTitle Title { get; set; }
        public HourBudget HourBudget { get; set; }
        public int RemainingHours { get; set; }
        public string Status { get; set; }
        public List<TeacherCourseViewModel> CurrentCourseOfferings { get; set; }
        public List<TeacherCourseViewModel> PastCourseOfferings { get; set; }
    }

    //Not currently used as the courses in teacher details use SimpleCourseViewModel
    //When we want to present different information from the course list we will need this
    //
    public class TeacherCourseViewModel
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string Code { get; set; }
        public TermYear TermYear { get; set; }
        public string Period { get; set; }
        public Teacher CourseResponsible { get; set; }
        public int TotalHours { get; set; }
        public int AllocatedHours { get; set; }
        public int RemainingHours { get; set; }
        public int TeacherAssignedHours { get; set; }
    }
}