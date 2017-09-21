using System.Collections.Generic;
using Agile.Models;

namespace Agile.DAL
{
	public class StaffingPlanInitializer
	{
		public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<StaffingPlanContext>
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
}