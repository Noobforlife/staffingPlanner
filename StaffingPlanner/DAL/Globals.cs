namespace StaffingPlanner.DAL
{
    public static class Globals
    {
        public static Role UserRole;
        public static string User;
    }

    public enum Role
    {
        Unauthorized = 0,
        Teacher = 1,
        DirectorOfStudies = 2,
    }
    

}