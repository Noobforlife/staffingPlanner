using System.Collections.Generic;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
	public class StaffingPlanInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<StaffingPlanContext>
	{
		protected override void Seed(StaffingPlanContext context)
		{
			var courses = new List<Course>
			{
				
			};

			courses.ForEach(c => context.Courses.Add(c));
			context.SaveChanges();

			var teachers = new List<Teacher>
			{

			};

			teachers.ForEach(t => context.Teachers.Add(t));
			context.SaveChanges();
		}
	}
}