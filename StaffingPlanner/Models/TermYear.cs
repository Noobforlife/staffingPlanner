using System;

namespace StaffingPlanner.Models
{
    public enum Term
    {
        Fall,
        Spring
    }

    public class TermYear
	{
        public Guid Id { get; set; }
        public Term Term { get; set; }
        public int Year { get; set; }

        private int GetTwoDigitYear()
        {
            return Year % 100;
        }

        public override string ToString()
        {
            if (Term == Term.Fall)
            {
                return "HT-" + GetTwoDigitYear();
            }
			return "VT-" + GetTwoDigitYear();
        }
    }
}