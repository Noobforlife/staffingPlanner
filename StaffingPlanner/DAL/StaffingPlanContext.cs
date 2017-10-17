using System.Data.Entity;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
    public class StaffingPlanContext : DbContext
    {
	    private StaffingPlanContext() : base("StaffingPlanContext") { }

		public static StaffingPlanContext GetContext()
        {
            return new StaffingPlanContext();
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherCourseWorkload> Workloads { get; set; }
        public DbSet<NonCourseWorkload> NonCourseWorkloads {get;set;}
        public DbSet<CourseOffering> CourseOfferings { get; set; }
        public DbSet<TermYear> TermYears { get; set; }
        public DbSet<TeacherTermEmployment> TeacherTermEmployment { get; set; }
        public DbSet<AcademicProfile> AcademicProfiles { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<TeacherTaskShare> TeacherTaskShare { get; set; }
		public DbSet<Comment> Comments { get; set; }
    }
}