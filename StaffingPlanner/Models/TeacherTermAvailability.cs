using System;

namespace StaffingPlanner.Models
{
	public class TeacherTermAvailability
	{
		public Guid Id { get; set; }
		public Teacher Teacher { get; set; }
        public TermYear TermYear { get; set; }
        public int Availability { get; set; }
    }
}
