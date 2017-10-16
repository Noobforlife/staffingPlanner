using System;
using System.Linq;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Models
{
    public class HourBudget
    {
        public Teacher Teacher { get; set; }
        public TermYear TermYear { get; set; }
        public int TermAvailability { get; set; }
        public int TotalTermHours { get; set; }
        public int TeachingHours { get; set; }
        public int ResearchHours { get; set; }
        public int AdminHours { get; set; }
        public int OtherHours { get; set; }

        public int TeachingShare { get; set; }
        public int ResearchShare { get; set; }
        public int AdminShare { get; set; }
        public int OtherShare { get; set; }

        public HourBudget(Teacher teacher, TermYear termYear)
        {
            Teacher = teacher;
            TermYear = termYear;

            //Get the availability for the term
            var db = StaffingPlanContext.GetContext();
            var availability = db.TeacherTermAvailability.Where(tta =>
                tta.Teacher.Id == teacher.Id &&
                tta.TermYear.Term == termYear.Term &&
                tta.TermYear.Year == termYear.Year)
                .AsEnumerable();
            TermAvailability = availability.Select(tta => tta.Availability).FirstOrDefault();

            //Multiply the total term hours (half of yearly) by term availability
            TotalTermHours = (int)(BaseYearlyHours / 2m * (TermAvailability / 100m));

            //Get the shares (% for teaching, research etc) for this teacher
            var result = db.TeacherTaskShare.Where(tts => tts.Teacher.Name == teacher.Name);
            TeacherTaskShare taskShare;
            if (result.Any())
            {
                var shares = result.Select(ap => new { TeachingShare = ap.TeachingShare, ResearchShare = ap.ResearchShare, AdminShare = ap.AdminShare, OtherShare = ap.OtherShare }).First();
                taskShare = new TeacherTaskShare
                {
                    Id = Guid.NewGuid(),
                    TeachingShare = shares.TeachingShare,
                    ResearchShare = shares.ResearchShare,
                    AdminShare = shares.AdminShare,
                    OtherShare = shares.OtherShare
                };
            }
            else
            {
                var profileShare = db.AcademicProfiles.Where(ap => ap.Title == teacher.AcademicTitle)
                .Select(ap => new { TeachingShare = ap.TeachingShare, ResearchShare = ap.ResearchShare, AdminShare = ap.AdminShare, OtherShare = ap.OtherShare });
                var shares = profileShare.First();
                taskShare = new TeacherTaskShare
                {
                    TeachingShare = shares.TeachingShare,
                    ResearchShare = shares.ResearchShare,
                    AdminShare = shares.AdminShare,
                    OtherShare = shares.OtherShare
                };
            }

            //Set the shares
            TeachingShare = (int)(taskShare.TeachingShare * 100);
            ResearchShare = (int)(taskShare.ResearchShare * 100);
            AdminShare = (int)(taskShare.AdminShare * 100);
            OtherShare = (int)(taskShare.OtherShare * 100);

            //Set the hours available for different tasks
            TeachingHours = (int)(TotalTermHours * taskShare.TeachingShare);
            ResearchHours = (int)(TotalTermHours * taskShare.ResearchShare);
            AdminHours = (int)(TotalTermHours * taskShare.AdminShare);
            OtherHours = (int)(TotalTermHours * taskShare.OtherShare);
        }

        public int BaseYearlyHours
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