namespace StaffingPlanner
{
    public static class Globals
    {
        public static bool isUserAuthorized = false;
        public static Role userRole;
    }

    public enum Role
    {
        DirectorOfStudies = 0,
        Teacher = 1,
    }
    

}