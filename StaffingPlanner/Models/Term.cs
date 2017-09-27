using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StaffingPlanner.Models
{
    public enum Term
    {
        Fall,
        Spring
    }

    /// <summary>
    /// Represents a term in a specific year,
    /// for example HT17 (the fall term 2017).
    /// </summary>
    public class TermYear
	{
        private int _year;
        private Term _term;

        public Term Term
        {
            get { return _term; }
        }

        public int Year
        {
            get { return _year; }
        }

        public TermYear(Term term, int year)
        {
            if (year < 1000 || year > 9999)
            {
                throw new ArgumentOutOfRangeException();
            }

            _term = term;

        }

        private int GetTwoDigitYear()
        {
            return _year % 100;
        }

        public override string ToString()
        {
            if (_term == Term.Fall)
            {
                return "HT" + GetTwoDigitYear().ToString();
            }
            else
            {
                return "VT" + GetTwoDigitYear().ToString();
            }
        }

    }
}