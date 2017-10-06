using System;

namespace StaffingPlanner.Models
{
    //The availability of a teacher for a given term
	public class TeacherTermAvailability
	{
		public Guid Id { get; set; }
		public Teacher Teacher { get; set; }
        public TermYear TermYear { get; set; }
        public int Availability { get; set; } //100 indicates full employment (100%)
    }
}
