using System.ComponentModel.DataAnnotations;

namespace StaffingPlanner.Models
{
	public class Course
	{
        [Key]
		public string Code { get; set; }
		public string Name { get; set; }

		public string TruncatedName
		{
			get
			{
				if (Name.Length > 20)
				{
					return Name.Substring(0, 19) + "...";
				}
				return Name;
			}
		}
    }
}