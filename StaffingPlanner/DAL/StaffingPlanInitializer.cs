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
				new Course{Id=0, Name="Agile"}
			};

			courses.ForEach(s => context.Courses.Add(s));
			context.SaveChanges();
		}
	}
}