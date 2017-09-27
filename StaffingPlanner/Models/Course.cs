using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StaffingPlanner.Models
{
	public class Course
	{
        [Key]
		public string Code { get; set; }
		public string Name { get; set; }
		public ICollection<CourseOffering> Offerings { get; set; }

		public CourseOffering GetOffering(TermYear termYear)
		{
			return Offerings.First(o => o.GetTermYear() == termYear);
		}

        public CourseOffering GetOffering(SchoolYear year)
        {
            return Offerings.First(o => o.GetTermYear() == year.FirstTerm || o.GetTermYear() == year.SecondTerm);
        }

    }
}