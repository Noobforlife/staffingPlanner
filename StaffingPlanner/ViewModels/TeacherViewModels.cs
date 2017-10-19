using System;
using StaffingPlanner.Models;

namespace StaffingPlanner.ViewModels
{

    public class SimpleTeacherViewModel
	{
		public Guid Id { get; set; }
        public string Name { get; set; }
		public AcademicTitle Title { get; set; }
        public int FallTermEmployment { get; set; }
        public int SpringTermEmployment { get; set; }
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

        //The total hours available for different tasks
        public TeacherTermAvailability FallBudget { get; set; }
        public TeacherTermAvailability SpringBudget { get; set; }

        //The hours actually allocated on differnt periods
        public TeacherTermWorkload FallWorkload { get; set; }
        public TeacherTermWorkload SpringWorkload { get; set; }
    }

    public class TeacherCourseViewModel
    {
		public Guid TeacherId { get; set; }
        public Guid OfferingId { get; set; }
		public Guid WorkloadId { get; set; }
        public string CourseName { get; set; }
        public string Code { get; set; }
        public TermYear TermYear { get; set; }
        public string Period { get; set; }
        public Teacher CourseResponsible { get; set; }
        public int TotalHours { get; set; }
        public int AllocatedHours { get; set; }
        public int RemainingHours { get; set; }
        public int TeacherAssignedHours { get; set; }
        public CourseState CourseState { get; set; }
        public string CourseStatus { get; set; }
    }

    public class TeacherCourseHistoryViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string TermYear { get; set; }
        public string Period { get; set; }
        public Teacher CourseResponsibe { get; set; }
        public int HoursTaught { get; set; }
    }
}