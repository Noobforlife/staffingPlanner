using System.Data.Entity;
using Agile.Models;

namespace Agile.DAL
{
	public class StaffingPlanContext : DbContext
	{
		public StaffingPlanContext() : base("StaffingPlanContext")
		{
		}

		public DbSet<Course> Courses { get; set; }
	}
}