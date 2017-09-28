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
  //      public CourseOffering CurrentOffering { get; set; }
		//public ICollection<CourseOffering> Offerings { get; set; }

  //      //public Course(string code, string name)
  //      //{
  //      //   CurrentOffering = GetOffering(term);
  //      //}

  //      public CourseOffering GetOffering(string term)
		//{
  //          //return Offerings.First(o => o.Term.ToLower() == term.ToLower());
  //          return Offerings.First();
  //      }

  //      public CourseOffering GetOffering(Guid offeringId)
  //      {
  //          return Offerings.First(o => o.Id == offeringId);
  //      }
    }
}