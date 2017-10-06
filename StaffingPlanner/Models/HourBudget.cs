using System;
using System.Linq;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Models
{
    public class HourBudget
    {
        public Teacher Teacher { get; set; }
        public TermYear FallTerm { get; set; }
        public TermYear SpringTerm { get; set; }
        public int FallAvailability { get; set; }
        public int SpringAvailability { get; set; }
        public int TotalHours { get; }
        public int RemainingHours { get; }
        public int TeachingHours { get; }
        public int ResearchHours { get; }
        public int AdminHours { get; }
        public int OtherHours { get; }

        public HourBudget(Teacher teacher, TermYear fallTerm, TermYear springTerm)
        {
            var db = StaffingPlanContext.GetContext();
            Teacher = teacher;
            FallTerm = fallTerm;
            SpringTerm = springTerm;

            FallAvailability = GetAvailability(fallTerm);
            SpringAvailability = GetAvailability(springTerm);

            decimal yearAvailability = (FallAvailability +
                                        (decimal)SpringAvailability) / 2m / 100m;

            //Multiply the total hours based on availability
            TotalHours = (int)(totalHoursPerYear * yearAvailability);

            //Get the shares (% for teaching, research etc) for this teachers academic title
            var result = db.AcademicProfiles.Where(ap => ap.Title == teacher.AcademicTitle)
                .Select(ap => new { TeachingShare = ap.TeachingShare, ResearchShare = ap.ResearchShare, AdminShare = ap.AdminShare, OtherShare = ap.OtherShare });
            var shares = result.First();

            //Set the hours available for different tasks
            TeachingHours = TotalHours * (int)shares.TeachingShare;
            ResearchHours = TotalHours * (int)shares.ResearchShare;
            AdminHours = TotalHours * (int)shares.AdminShare;
            OtherHours = TotalHours * (int)shares.OtherShare;
        }

        private int GetAvailability(TermYear termYear)
        {
            //Get the availability for the term
            var db = StaffingPlanContext.GetContext();
            var availability = db.TeacherTermAvailability.Where(tta =>
                tta.Teacher.Id == Teacher.Id &&
                tta.TermYear.Term == termYear.Term &&
                tta.TermYear.Year == termYear.Year)
                .AsEnumerable();
            var termAvailability = availability.Select(tta => tta.Availability).FirstOrDefault();
            return termAvailability;
        }

        private int totalHoursPerYear
        {
            get
            {
                var year = int.Parse("19" + Teacher.PersonalNumber.Substring(0, 2));
                var month = int.Parse(Teacher.PersonalNumber.Substring(2, 2));
                var day = int.Parse(Teacher.PersonalNumber.Substring(4, 2));
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