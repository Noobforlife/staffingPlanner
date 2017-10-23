using System;
using StaffingPlanner.Models;
using System.Collections.Generic;

namespace StaffingPlanner.DAL
{
    public static class Globals
    {
        public static Dictionary<string, User> SessionUser = new Dictionary<string, User>();
        public static AcademicYear CurrentAcademicYear;
        public static TermYear CurrentTerm;

        public static string isActiveTerm(Term term, TermYear current)
        {
            if (current.Term == term) {
                return "active";
            }
                return null;
        }


    }

    public enum Role
    {
        Unauthorized = 0,
        Teacher = 1,
        DirectorOfStudies = 2,
    }

}