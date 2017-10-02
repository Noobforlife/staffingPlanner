using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

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

        public override bool Equals(object obj)
        {
            var t = obj as TermYear;
            return t != null && t.Term == Term && t.Year == Year;
        }

    }
}

#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
