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
    }

    public enum Role
    {
        Unauthorized = 0,
        Teacher = 1,
        DirectorOfStudies = 2,
    }
    

}