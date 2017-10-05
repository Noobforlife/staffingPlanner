using System;
using System.Linq;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Models
{
    public class HourBudget
    {
        public Teacher teacher { get; set; }
        public int TotalHours { get; }
        public int RemainingHours { get; }
        public int TeachingHours { get; }
        public int ResearchHours { get; }
        public int AdminHours { get; }
        public int OtherHours { get; }

        public HourBudget(Teacher teacher)
        {
            this.teacher = teacher;

            //Get the shares (% for teaching, research etc) for this teachers academic title
            var db = StaffingPlanContext.GetContext();
            var result = db.AcademicProfiles.Where(ap => ap.Title == teacher.AcademicTitle)
                .Select(ap => new { TeachingShare = ap.TeachingShare, ResearchShare = ap.ResearchShare, AdminShare = ap.AdminShare, OtherShare = ap.OtherShare });
            var shares = result.First();

            TotalHours = totalHours;
            TeachingHours = TotalHours * (int)shares.TeachingShare;
            ResearchHours = TotalHours * (int)shares.ResearchShare;
            AdminHours = TotalHours * (int)shares.AdminShare;
            OtherHours = TotalHours * (int)shares.OtherShare;
        }

        private int totalHours
        {
            get
            {
                var year = int.Parse("19" + teacher.PersonalNumber.Substring(0, 2));
                var month = int.Parse(teacher.PersonalNumber.Substring(2, 2));
                var day = int.Parse(teacher.PersonalNumber.Substring(4, 2));
                var birthdate = new DateTime(year, month, day);
                var age = new DateTime().Subtract(birthdate);

                if (age.Days / 365 > 40)
                {
                    return 1700;
                }
                if (age.Days / 365 > 30 && age.Days / 365 <= 40)
                {
                    return 1735;
                }

                return 1756;
            }
        }

    }
}