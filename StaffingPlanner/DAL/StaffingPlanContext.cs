﻿using System.Data.Entity;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
	public class StaffingPlanContext : DbContext
	{
		private static readonly StaffingPlanContext context = new StaffingPlanContext();

		public static StaffingPlanContext GetContext()
		{
			return context;
		}

		private StaffingPlanContext() : base("StaffingPlanContext")
		{
		}

		public DbSet<Course> Courses { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
	}
}