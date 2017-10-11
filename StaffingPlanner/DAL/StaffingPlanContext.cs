using System.Data.Entity;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
    public class StaffingPlanContext : DbContext
    {
        public static StaffingPlanContext GetContext()
        {
            return new StaffingPlanContext();
        }

        private StaffingPlanContext() : base("StaffingPlanContext")
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherCourseWorkload> Workloads { get; set; }
        public DbSet<CourseOffering> CourseOfferings { get; set; }
        public DbSet<TermYear> TermYears { get; set; }
        public DbSet<TeacherTermAvailability> TeacherTermAvailability { get; set; }
        public DbSet<AcademicProfile> AcademicProfiles { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }

    }
}