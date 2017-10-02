using System;
using System.Linq;
using StaffingPlanner.DAL;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

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

		public int TotalHours
		{
			get
			{
				var year = int.Parse("19" + PersonalNumber.Substring(0, 2));
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
		}

		public int RemainingHours
		{
			get
			{
				var db = StaffingPlanContext.GetContext();

				var hours = db.Workloads
					.Where(w => w.Teacher.Id.Equals(Id))
					.ToList()
					.Sum(w => w.Workload);

				return TotalHours - hours;
			}
		}

        public override bool Equals(object obj)
        {
            var t = obj as Teacher;
            return t != null && t.Id == Id;
        }
    }
}

#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()