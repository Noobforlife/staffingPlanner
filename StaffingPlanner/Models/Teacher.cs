using System;
using System.Linq;
using StaffingPlanner.DAL;
using System.ComponentModel.DataAnnotations.Schema;

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

        public HourBudget GetHourBudget(TermYear fallTerm, TermYear springTerm)
        {
            return new HourBudget(this, fallTerm, springTerm);
        }

        //Gets all allocated hours in history regardless of term or year, fix?
        public int AllocatedHours
        {
            get
            {
                var db = StaffingPlanContext.GetContext();

                var hours = db.Workloads
                    .Where(w => w.Teacher.Id.Equals(Id))
                    .ToList()
                    .Sum(w => w.Workload);

                return hours;
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