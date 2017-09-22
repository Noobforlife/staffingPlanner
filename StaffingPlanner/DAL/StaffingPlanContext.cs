using System.Data.Entity;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
	public class StaffingPlanContext : DbContext
	{
		public StaffingPlanContext() : base("StaffingPlanContext")
		{
		}

		public DbSet<Course> Courses { get; set; }
	}
}