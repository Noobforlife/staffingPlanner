using System.Collections.Generic;
using StaffingPlanner.Models;
using System;

namespace StaffingPlanner.DAL
{
    public class StaffingPlanInitializer : System.Data.Entity.DropCreateDatabaseAlways<StaffingPlanContext>
    {
        protected override void Seed(StaffingPlanContext context)
        {
            TermYear fallTerm = new TermYear { Id = Guid.NewGuid(), Term = Term.Fall, Year = 2017 };
            TermYear springTerm = new TermYear { Id = Guid.NewGuid(), Term = Term.Spring, Year = 2018 };
            context.TermYears.Add(fallTerm);
            context.TermYears.Add(springTerm);
            context.SaveChanges();

            var teachers = new List<Teacher>
            {
                new Teacher()
                {
                    Name = "Tomas Eklund",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "740905-2886",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = true,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Andreas Hamfeldt",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "610427-1541",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Mats Edenius",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "610427-1541",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Pär Ågerfalk",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "740905-2886",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Anneli Edman",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "560129-7352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Barbro Funseth",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "560129-7352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Claes Thorén",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Franck Tétard",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Anton Backe",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "920610-8361",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Christer Stuxberg",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Görkem Pacaci",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Mustafa Mudassir Imran",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "740905-2886",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Christopher Ohkravi",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Asma Rafiq",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Sofie Roos",
                    AcademicTitle = AcademicTitle.Amanuens,
                    PersonalNumber = "920610-8361",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },

                new Teacher()
                {
                    Name = "Daniel Wallman",
                    AcademicTitle = AcademicTitle.Amanuens,
                    PersonalNumber = "920610-8361",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                    TermEmployment = DataGen.GetEmploymentDictionary(fallTerm, 100, springTerm, 100)
                },


            };

            teachers.ForEach(t => context.Teachers.Add(t));
            context.SaveChanges();

            //Populating database with courses
            var courses = new List<Course>
            {
                new Course()
                {
                    Code = "2IS100",
                    Name = "Agile methods",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 2)
                },

                new Course()
                {
                    Code = "3IS834",
                    Name = "Algoritmer och datastrukturer",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS872",
                    Name = "Algoritmik",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS625",
                    Name = "Användbarhet och e-tjänster",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS837",
                    Name = "Artificial Intelligence ",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3FE220",
                    Name = "Corporate communication",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS155",
                    Name = "Databaser",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS237",
                    Name = "Datamining och Data Warehousing",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS782",
                    Name = "Declarative Problem Solving Methods",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU826",
                    Name = "Dotnet-programmering",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS991",
                    Name = "eTjänster och webbprogrammering",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU264",
                    Name = "Examensarbete",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "2IS015",
                    Name = "Forskningsmetod",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU812",
                    Name = "Grundläggande MDI",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS887",
                    Name = "Informationsinfrastruktur",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS816",
                    Name = "Internetbaserade system",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU937",
                    Name = "Introduktion till management, kommunikation och IT",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU049",
                    Name = "IT och strategi",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS978",
                    Name = "Knowledge Management ",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU415",
                    Name = "Logik",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU575",
                    Name = "Master Thesis",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3MU677",
                    Name = "Multimedia",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS102",
                    Name = "Objektorienterad programmering I",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "3IS202",
                    Name = "Objektorienterad programmering II",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },

                new Course()
                {
                    Code = "2AD339",
                    Name = "Software Engineering",
                    //Offerings = DataGen.GetOfferings(teachers[DataGen.rnd.Next(0, teachers.Count)], 1)
                },
            };
            courses.ForEach(c => context.Courses.Add(c));
            context.SaveChanges();

            //Populating database with TermYears
            var TermYrs = new List<TermYear> {
                new TermYear { Id = Guid.NewGuid(), Term = Term.Fall, Year = 2017 },
                new TermYear { Id = Guid.NewGuid(), Term = Term.Spring, Year = 2018 }
            };
            TermYrs.ForEach(c => context.TermYears.Add(c));
            context.SaveChanges();

            //Populating database with courseofferings
            foreach (var c in courses) {
                var offering = DataGen.CreateOffering(teachers[DataGen.rnd.Next(0, teachers.Count)], c, TermYrs[DataGen.rnd.Next(0, TermYrs.Count)]);
                context.CourseOfferings.Add(offering);
            }
            context.SaveChanges();
                       

            //Populating database with workloads
            foreach (var c in context.CourseOfferings)
            {
                var workload = DataGen.CreateWorkload(teachers[DataGen.rnd.Next(0, teachers.Count)], c);
                context.Workloads.Add(workload);
            }            
            context.SaveChanges();                  

        }
    }
}