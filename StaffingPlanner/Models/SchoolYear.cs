using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffingPlanner.Models
{
    public class SchoolYear
    {
        public TermYear FirstTerm { get; }
        public TermYear SecondTerm { get; }

        public int StartYear { get; }
        public int EndYear { get; }

        public SchoolYear(int startYear)
        {
            if (startYear < 1000 || startYear > 9999)
            {
                throw new ArgumentOutOfRangeException();
            }

            FirstTerm = new TermYear(Term.Fall, startYear);
            SecondTerm = new TermYear(Term.Spring, startYear + 1);
            StartYear = startYear;
            EndYear = startYear + 1;
        }


        public static TermYear GetOtherTerm(TermYear termyear)
        {
            if (termyear.Term == Term.Fall)
            {
                return new TermYear(Term.Spring, termyear.Year + 1);
            }
            else
            {
                return new TermYear(Term.Fall, termyear.Year - 1);
            }
        }
    }
}