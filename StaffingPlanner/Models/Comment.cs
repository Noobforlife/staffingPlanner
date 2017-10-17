using System;

namespace StaffingPlanner.Models
{
	public class Comment
	{
		public Guid Id { get; set; }
		public Guid ConnectedTo { get; set; }
		public string Message { get; set; }
	}
}