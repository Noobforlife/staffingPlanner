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

        public TermYear(Term term, int yy)
        {
            _term = term;

            if (yy >= 0 && yy < 100)
            {
                _year = yy;
            }
            if (yy >= 100)
            {
                _year = yy % 100;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            if (_term == Term.Fall)
            {
                return "HT" + _year.ToString();
            }
            else
            {
                return "VT" + _year.ToString();
            }
        }

        public static TermYear GetNextTerm(TermYear termyear)
        {
            if (termyear.Term == Term.Fall)
            {
                return new TermYear(Term.Spring, termyear.Year + 1);
            }
            else
            {
                return new TermYear(Term.Fall, termyear.Year);
            }
        }
    }
}