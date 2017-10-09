using System;
using System.Collections.Generic;
using StaffingPlanner.Models;

namespace StaffingPlanner.DAL
{
    public static class DataGen
    {
        public static Random Rnd = new Random();

        public static TeacherTermAvailability GetTeacherTermAvailability(Teacher teacher, TermYear termyear, int employmentLevel)
        {
            return new TeacherTermAvailability
            {
                Id = Guid.NewGuid(),
                Teacher = teacher,
                TermYear = termyear,
                Availability = employmentLevel
            };
        }

        private static readonly List<float> Hst = new List<float>()
        {
            1,9f, 10,25f, 3,25f, 6f, 27,5f, 7,5f, 7,5f, 10,25f, 1,9f, 1,9f, 7,5f, 13,75f, 5f, 6,25f, 7,5f
        };

        private static readonly List<double> Credits = new List<double>()
        {
            7.5,
            15.0
        };

        public static CourseOffering CreateOffering(Teacher courseResponsible, Course course, TermYear termyear, CourseState state)
        {
            return new CourseOffering {
                Id =Guid.NewGuid(),
                Course = course,
                TermYear = termyear,
                Credits = Credits[Rnd.Next(0, Credits.Count)],
                Periods = RandomPeriod(),
                TotalHours = Rnd.Next(300, 800),
                CourseResponsible = courseResponsible,
                State = state,
                NumStudents = Rnd.Next(10, 80),
                RegisteredStudents = Rnd.Next(10,79),
                PassedStudents = Rnd.Next(0,50),
                HST = Hst[Rnd.Next(0, Hst.Count)]
            };
        }

        private static Period RandomPeriod()
        {
            var values = Enum.GetValues(typeof(Period));
            var randomCredit = (Period)values.GetValue(Rnd.Next(values.Length));
            return randomCredit;
        }
               
        public static TeacherCourseWorkload CreateWorkload(Teacher teacher, CourseOffering course)
        {
            int min = 50;
            int max = 300;
            if (teacher.AcademicTitle == AcademicTitle.Amanuens || teacher.AcademicTitle == AcademicTitle.Doktorand)
            {
                max = 120;
            }
            else if (teacher.AcademicTitle == AcademicTitle.Professor)
            {
                max = 200;
            }
            else if (teacher.AcademicTitle == AcademicTitle.Lektor || teacher.AcademicTitle == AcademicTitle.Adjunkt)
            {
                min = 150;
            }

            return new TeacherCourseWorkload
            {
                Id = Guid.NewGuid(),
                Course = course,
                Teacher = teacher,
                Workload = Rnd.Next(min, max)
            };
        }

    }
}
