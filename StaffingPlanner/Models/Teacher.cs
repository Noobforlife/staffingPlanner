using System;
using System.Collections.Generic;
using System.Linq;
using StaffingPlanner.DAL;

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
        public Dictionary<TermYear, EmploymentLevel> TermEmployment { get; set; }


        public void AddTermEmployment(TermYear term, EmploymentLevel employment)
        {
            TermEmployment.Add(term, employment);
        }
        public EmploymentLevel GetEmployment(TermYear term)
		{
            return TermEmployment[term];
		}

		// Alter to take into account any changes in workload
		public int GetTotalHours()
		{
			var year = int.Parse("19" + PersonalNumber.Substring(0,2));
			var month = int.Parse(PersonalNumber.Substring(2, 2));
			var day = int.Parse(PersonalNumber.Substring(4, 2));
			var birthdate = new DateTime(year, month, day);
			var age = new DateTime().Subtract(birthdate);

			if (age.Days / 365 > 40)
			{
				return 1700;
			}
			if (age.Days / 365 > 30 && age.Days / 365 <= 40)
			{
				return 1735;
			}
		
			return 1756;
		}

		// Alter to take into account how much teaching is to be done, and if there is some decrease in workload
		public int GetRemainingHours()
		{
			var db = StaffingPlanContext.GetContext();
			var hours = db.Workloads.Where(w => w.Teacher == this).Select(w => w.Workload).Sum();

			return GetTotalHours() - hours;
		}
	}
}