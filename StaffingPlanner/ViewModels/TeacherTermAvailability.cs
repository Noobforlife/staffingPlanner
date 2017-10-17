using System;
using System.Linq;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Models
{
    //Contains all availability data for a teacher for a given term
    public class TeacherTermAvailability
    {
        public Teacher Teacher { get; set; }
        public TermYear TermYear { get; set; }
        public int TermEmployment { get; set; }
        public int TotalTermHours { get; set; }

        public int TeachingHours { get; set; }
        public int ResearchHours { get; set; }
        public int AdminHours { get; set; }
        public int OtherHours { get; set; }

        public int TeachingShare { get; set; }
        public int ResearchShare { get; set; }
        public int AdminShare { get; set; }
        public int OtherShare { get; set; }

        public TeacherTermAvailability(Teacher teacher, TermYear termYear)
        {
            Teacher = teacher;
            TermYear = termYear;

            //Get the availability for the term
            var db = StaffingPlanContext.GetContext();
            var employment = db.TeacherTermEmployment.Where(tta =>
                tta.Teacher.Id == teacher.Id &&
                tta.TermYear.Term == termYear.Term &&
                tta.TermYear.Year == termYear.Year)
                .AsEnumerable();
            TermEmployment = employment.Select(tta => tta.Employment).FirstOrDefault();

            //Multiply the total term hours (half of yearly) by term availability
            TotalTermHours = (int)(BaseYearlyHours / 2m * (TermEmployment / 100m));

            var taskShare = SetShares();

            //Set the hours available for different tasks
            TeachingHours = (int)(TotalTermHours * taskShare.TeachingShare);
            ResearchHours = (int)(TotalTermHours * taskShare.ResearchShare);
            AdminHours = (int)(TotalTermHours * taskShare.AdminShare);
            OtherHours = (int)(TotalTermHours * taskShare.OtherShare);
        }
        
        private TeacherTaskShare SetShares()
        {
            var db = StaffingPlanContext.GetContext();

            //Get the shares (% for teaching, research etc) for this teacher
            var result = db.TeacherTaskShare.Where(tts => tts.Teacher.Name == Teacher.Name);
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
                var profileShare = db.AcademicProfiles.Where(ap => ap.Title == Teacher.AcademicTitle)
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
            return taskShare;
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