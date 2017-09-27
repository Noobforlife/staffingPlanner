using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Linq;

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
            _year = year;
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

        public static TermYear StringToTermYear(string termYearString)
        {
            if (termYearString.Length != 4)
            {
                throw new ArgumentException("Has to be in format HT17, VT18 etc.");
            }
            string termString = termYearString.Substring(0, 2);
            string yearString = termYearString.Substring(2, 2);
            int year = int.Parse(yearString) + 2000;

            if (termString == "HT")
            {
                return new TermYear(Term.Fall, year);
            }
            else if (termString == "VT")
            {
                return new TermYear(Term.Spring, year);
            }
            else
            {
                throw new ArgumentException("Has to be in format HT17, VT18 etc.");
            }

        }

    }
}