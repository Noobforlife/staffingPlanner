using System.Linq;
using StaffingPlanner.Models;
using StaffingPlanner.DAL;

namespace StaffingPlanner.ViewModels
{
    public class TeacherTermWorkload
    {
        public int TotalTermWorkload { get; set; }
        public int NonCourseWorkload { get; set; }
        public int Period1Workload { get; set; }
        public int Period2Workload { get; set; }
        public int Period3Workload { get; set; }
        public int Period4Workload { get; set; }

        public TeacherTermWorkload(Teacher teacher, TermYear term)
        {
            //Get the teachers workloads for the term
            var db = StaffingPlanContext.GetContext();
            var teacherWorkloads = db.Workloads.Where(wl => wl.Teacher.Id == teacher.Id 
            && wl.Course.TermYear.Year == term.Year
            && wl.Course.TermYear.Term == term.Term);

            //Get the total of course workload hours
            TotalTermWorkload = teacherWorkloads.Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);
            //Add the non course workload hours
            NonCourseWorkload = db.NonCourseWorkloads.Where(wl => wl.Teacher.Id == teacher.Id && wl.TermYear.Id == term.Id).Select(wl => wl.Workload).DefaultIfEmpty(0).First();
            TotalTermWorkload += NonCourseWorkload;

            var allTermWorkload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.AllPeriods)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);
            var firstHalfWorkload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P1P2)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);
            var secondHalfWorkload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P3P4)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl);

            Period1Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P1)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + firstHalfWorkload / 2 + allTermWorkload / 4;
            Period2Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P2)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + firstHalfWorkload / 2 + allTermWorkload / 4;
            Period3Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P3)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + secondHalfWorkload / 2 + allTermWorkload / 4;
            Period4Workload = teacherWorkloads.Where(wl => wl.Course.Periods == Period.P4)
                .Select(wl => wl.Workload).DefaultIfEmpty(0).Sum(wl => wl) + secondHalfWorkload / 2 + allTermWorkload / 4;
        }
    }
}