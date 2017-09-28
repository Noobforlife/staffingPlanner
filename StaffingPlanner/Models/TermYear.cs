using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using System.Linq;

namespace StaffingPlanner.Models
{
    public enum Term
    {
        [Description("HT")]
        Fall,
        [Description("VT")]
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
                return "HT" + GetTwoDigitYear();
            }
            else
            {
                return "VT" + GetTwoDigitYear();
            }
        }

    }
}