namespace StaffingPlanner.Models
{
    public enum CourseState
    {
		Draft,
        Ongoing,
        Planned,
        Completed
    }

    public enum Period
    {
        P1,
        P1P2,
        AllPeriods,
        P2,
        P3,
        P3P4,
        P4
    }

    public static class EnumToString
    {
        public static string PeriodToString(Period period)
        {
            switch (period)
            {
                case Period.P1:
                    return "P1";
                case Period.P2:
                    return "P2";
                case Period.P3:
                    return "P3";
                case Period.P4:
                    return "P4";
                case Period.P1P2:
                    return "P1-2";
                case Period.P3P4:
                    return "P3-4";
                default:
                    return "P1-4";
            }
        }
    }
}