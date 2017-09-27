﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StaffingPlanner.Models;
using System.Collections;

namespace StaffingPlanner.DAL
{
    public static class DataGen
    {
        public static Random rnd = new Random();

        private static List<TermYear> termList = new List<TermYear>()
        {
            new TermYear(Term.Fall, 2017),
            new TermYear(Term.Spring, 2018)
        };

        private static List<float> hst = new List<float>()
        {
            1,9f, 10,25f, 3,25f, 6f, 27,5f, 7,5f, 7,5f, 10,25f, 1,9f, 1,9f, 7,5f, 13,75f, 5f, 6,25f, 7,5f
        };

        public static CourseOffering GetOffering(Teacher courseResponsible)
        {
            return new CourseOffering()
            {
                Id = Guid.NewGuid(),
                TermYear = termList[rnd.Next(0, termList.Count)],
                Credits = GetRandomCredit(),
                CourseResponsible = courseResponsible,
                NumStudents = rnd.Next(10, 80),
                HST = hst[rnd.Next(0, hst.Count)],
                Periods = new List<Period>() { Period.P1, Period.P2 },
                Budget = rnd.Next(20, 200),

            };
        }

        public static List<CourseOffering> GetOfferings(Teacher courseResponsible, int number)
        {
            List<CourseOffering> courseOfferings = new List<CourseOffering>();

            for (int i = 0; i < number; i++)
            {
                courseOfferings.Add(GetOffering(courseResponsible));
            }

            return courseOfferings;
        }

        private static Credits GetRandomCredit()
        {
            Array values = Enum.GetValues(typeof(Credits));
            Credits randomCredit = (Credits)values.GetValue(rnd.Next(values.Length));
            return randomCredit;
        }

    }
}
