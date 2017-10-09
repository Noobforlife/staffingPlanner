using System;
using System.Linq;
using StaffingPlanner.Models;
using StaffingPlanner.DAL;
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

        //The total hours available for different tasks
        public HourBudget FallBudget { get; set; }
        public HourBudget SpringBudget { get; set; }

        //The hours actually allocated on differnt periods
        public TeacherPeriodWorkload FallPeriodWorkload { get; set; }
        public TeacherPeriodWorkload SpringPeriodWorkload { get; set; }

        public List<TeacherCourseViewModel> CurrentCourseOfferings { get; set; }
        public List<TeacherCourseViewModel> OtherCourseOfferings { get; set; }

    }

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

    public class TeacherPeriodWorkload
    {
        public int Period1Workload { get; set; }
        public int Period2Workload { get; set; }
        public int Period3Workload { get; set; }
        public int Period4Workload { get; set; }

        public TeacherPeriodWorkload(Teacher teacher, TermYear term)
        {
            var db = StaffingPlanContext.GetContext();
            var teacherWorkloads = db.Workloads.Where(wl => wl.Teacher.Id == teacher.Id);

            var allTermWorkload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.AllPeriods)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);
            var firstHalfWorkload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P1P2)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);
            var secondHalfWorkload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P3P4)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);

            Period1Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P1)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + firstHalfWorkload / 2 + allTermWorkload / 4;
            Period1Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P2)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + firstHalfWorkload / 2 + allTermWorkload / 4;
            Period1Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P3)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + secondHalfWorkload / 2 + allTermWorkload / 4;
            Period1Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P4)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + secondHalfWorkload / 2 + allTermWorkload / 4;
        }
    }
}