using StaffingPlanner.DAL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace StaffingPlanner.Models
{
    public class AcademicYear
    {
        public Guid Id { get; set; }
        public virtual TermYear StartTerm { get; set; }
        public virtual TermYear EndTerm { get; set; }

        public static AcademicYear GetCurrentYear()
        {
            var db = StaffingPlanContext.GetContext();

            AcademicYear currentYear;

            var now = DateTime.Now;
            if (now.Month > 7)
            {
                currentYear = db.AcademicYears.Where(ay => ay.StartTerm.Year == now.Year).FirstOrDefault();
            }
            else
            {
                currentYear = db.AcademicYears.Where(ay => ay.EndTerm.Year == now.Year).FirstOrDefault();
            }

            return currentYear;
        }

    }

}