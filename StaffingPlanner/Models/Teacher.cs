using System;
using System.Collections.Generic;
using System.Linq;
using StaffingPlanner.DAL;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Dictionary<TermYear, int> TermEmployment { get; set; }

	}
}