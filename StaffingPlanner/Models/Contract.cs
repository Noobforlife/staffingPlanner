using System;

namespace StaffingPlanner.Models
{
	public class Contract
	{
		public Guid Id { get; set; }
		public string SchoolYear { get; set; }
		public int FallWork { get; set; }
		public int SpringWork { get; set; }
	}
}