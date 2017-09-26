using StaffingPlanner.Models;

namespace StaffingPlanner
{
    public static class Globals
    {
        public static Role userRole;
        public static string user;
    }

    public enum Role
    {
        Unauthorized = 0,
        Teacher = 1,
        DirectorOfStudies = 2,
    }
    

}