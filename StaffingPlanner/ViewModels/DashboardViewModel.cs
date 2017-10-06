using StaffingPlanner.Models;
using System;
using System.Collections.Generic;

namespace StaffingPlanner.ViewModels
{
	public class DashboardViewModel {
		public bool DoS { get; set; }
		public TopPanelViewModel TopPanel { get; set; }
		public List<DashboardCourseViewModel> FallCourses { get; set; }
		public List<DashboardCourseViewModel> SpringCourses { get; set; }
	}

	public class TopPanelViewModel
	{
		public int TotalRemaining { get; set; }
		public int ProfessorRemaining { get; set; }
		public int LektorRemaining { get; set; }
		public int AdjunktRemaining { get; set; }
		public int DoktorandRemaining { get; set; }
		public int AmanuensRemaining { get; set; }
		public int AssistentRemaining { get; set; }
	}

	public class DashboardCourseViewModel
	{
        public Guid Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int PeriodsBefore { get; set; }
		public int PeriodsDuration { get; set; }
		public int PeriodsAfter { get; set; }
        public string Status { get; set; }
        public Teacher CourseResponsible { get; set; }

        public Period Periods
		{
			set
			{
				PeriodsBefore = GetPeriodsBefore(value);
				PeriodsDuration = GetPeriodsDuring(value);
				PeriodsAfter = GetPeriodsAfter(value);
			}
		}

		private static int GetPeriodsBefore(Period period)
		{
			switch (period)
			{
				case Period.P1:
				case Period.P1P2:
				case Period.AllPeriods:
					return 0;
				case Period.P2:
					return 1;
				case Period.P3:
				case Period.P3P4:
					return 2;
				case Period.P4:
					return 3;
				default:
					return -1;
			}
		}

		private static int GetPeriodsDuring(Period period)
		{
			switch (period)
			{
				case Period.P1:
				case Period.P2:
				case Period.P3:
				case Period.P4:
					return 1;
				case Period.P1P2:
				case Period.P3P4:
					return 2;
				case Period.AllPeriods:
					return 4;
				default:
					return -1;
			}
		}

		private static int GetPeriodsAfter(Period period)
		{
			switch (period)
			{
				case Period.P1:
					return 3;
				case Period.P2:
				case Period.P1P2:
					return 2;
				case Period.P3:
					return 1;
				case Period.P4:
				case Period.P3P4:
				case Period.AllPeriods:
					return 0;
				default:
					return -1;
			}
		}
	}
}