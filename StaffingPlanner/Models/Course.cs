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

		public CourseOffering GetOffering(string schoolYear)
		{
			return Offerings.First(o => o.SchoolYear == schoolYear);
		}

        public CourseOffering GetOffering(Guid offeringId)
        {
            return Offerings.First(o => o.Id == offeringId);
        }
    }
}