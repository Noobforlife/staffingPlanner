using System;
using System.Collections.Generic;
using System.Linq;

namespace StaffingPlanner.Models
{
	public class Teacher
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string PersonalNumber { get; set; }
		public string Email { get; set; }
		public bool DirectorOfStudies { get; set; }
		public AcademicTitle AcademicTitle { get; set; }
		public ICollection<Contract> Contracts { get; set; }

		public Contract GetContract(string schoolYear)
		{
			return Contracts.First(c => c.SchoolYear == schoolYear);
		}

		public int GetTotalHours()
		{
			return 0;
		}

		public int GetRemainingHours()
		{
			return 0;
		}
	}
}