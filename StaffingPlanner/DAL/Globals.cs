using System;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
    public static class Globals
    {
        public static Role UserRole;
        public static string User;
	    public static Guid UserId;
        public static AcademicYear CurrentAcademicYear;
        public static TermYear CurrentTerm;

        public static string isActiveTerm(Term term, TermYear current) {
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