using System;

namespace StaffingPlanner.Models
{
    //The employment of a teacher for a given term
	public class TeacherTermEmployment
	{
		public Guid Id { get; set; }
		public Teacher Teacher { get; set; }
        public TermYear TermYear { get; set; }
        public int Employment { get; set; } //100 indicates full employment (100%)
    }
}
