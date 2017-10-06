using System.Collections.Generic;
using StaffingPlanner.Models;
using System;

namespace StaffingPlanner.DAL
{
    public class StaffingPlanInitializer : System.Data.Entity.DropCreateDatabaseAlways<StaffingPlanContext>
    {
        protected override void Seed(StaffingPlanContext context)
        {
            //Populating database with TermYears
            var VT17 = new TermYear { Id = Guid.NewGuid(), Term = Term.Spring, Year = 2017 };
            var HT17 = new TermYear { Id = Guid.NewGuid(), Term = Term.Fall, Year = 2017 };
            var VT18 = new TermYear { Id = Guid.NewGuid(), Term = Term.Spring, Year = 2018 };
            var termYears = new List<TermYear> {
                HT17,
                VT18,
                VT17
            };
            termYears.ForEach(c => context.TermYears.Add(c));
            context.SaveChanges();

            //Populating database with teachers
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Name = "Tomas Eklund",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "740905-2886",
                    Email = "tomas.eklund@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = true,
                },

                new Teacher
                {
                    Name = "Andreas Hamfeldt",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "610427-1541",
                    Email = "andreas.hamfeldt@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Mats Edenius",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "610427-1541",
                    Email = "mats.edenium@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Pär Ågerfalk",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "740905-2886",
                    Email = "par.agerfalk@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Anneli Edman",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "560129-7352",
                    Email = "anneli.edman@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Barbro Funseth",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "560129-7352",
                    Email = "barbro.funseth@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Claes Thorén",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "821111-8352",
                    Email = "claes.thoren@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Franck Tétard",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "821111-8352",
                    Email = "franck.tetard@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Anton Backe",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "920610-8361",
                    Email = "anton.backe@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Christer Stuxberg",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "821111-8352",
                    Email = "christer.stuxberg@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Görkem Pacaci",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "821111-8352",
                    Email = "gorkem.pacaci@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Mustafa Mudassir Imran",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "740905-2886",
                    Email = "mustafa.imran@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Christopher Ohkravi",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "821111-8352",
                    Email = "christopher.ohkravi@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Asma Rafiq",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "821111-8352",
                    Email = "asma.rafiq@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Sofie Roos",
                    AcademicTitle = AcademicTitle.Amanuens,
                    PersonalNumber = "920610-8361",
                    Email = "sofie.roos@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },

                new Teacher
                {
                    Name = "Daniel Wallman",
                    AcademicTitle = AcademicTitle.Amanuens,
                    PersonalNumber = "920610-8361",
                    Email = "daniel.wallman@im.uu.se",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false,
                },


            };

            teachers.ForEach(t => context.Teachers.Add(t));
            context.SaveChanges();

            //Populating database with courses
            var courses = new List<Course>
            {
                new Course
                {
                    Code = "2IS100",
                    Name = "Agile methods",
                },

                new Course
                {
                    Code = "3IS834",
                    Name = "Algoritmer och datastrukturer",
                },

                new Course
                {
                    Code = "3IS872",
                    Name = "Algoritmik",
                },

                new Course
                {
                    Code = "3IS625",
                    Name = "Användbarhet och e-tjänster",
                },

                new Course
                {
                    Code = "3IS837",
                    Name = "Artificial Intelligence ",
                },

                new Course
                {
                    Code = "3FE220",
                    Name = "Corporate communication",
                },

                new Course
                {
                    Code = "3IS155",
                    Name = "Databaser",
                },

                new Course
                {
                    Code = "3IS237",
                    Name = "Datamining och Data Warehousing",
                },

                new Course
                {
                    Code = "3IS782",
                    Name = "Declarative Problem Solving Methods",
                },

                new Course
                {
                    Code = "3MU826",
                    Name = "Dotnet-programmering",
                },
		
                new Course
                {
                    Code = "3IS991",
                    Name = "eTjänster och webbprogrammering",
                },

                new Course
                {
                    Code = "3MU264",
                    Name = "Examensarbete",
                },

                new Course
                {
                    Code = "2IS015",
                    Name = "Forskningsmetod",
                },

                new Course
                {
                    Code = "3MU812",
                    Name = "Grundläggande MDI",
                },

                new Course
                {
                    Code = "3IS887",
                    Name = "Informationsinfrastruktur",
                },

                new Course
                {
                    Code = "3IS816",
                    Name = "Internetbaserade system",
                },

                new Course
                {
                    Code = "3MU937",
                    Name = "Introduktion till management, kommunikation och IT",
                },

                new Course
                {
                    Code = "3MU049",
                    Name = "IT och strategi",
                },

                new Course
                {
                    Code = "3IS978",
                    Name = "Knowledge Management ",
                },

                new Course
                {
                    Code = "3MU415",
                    Name = "Logik",
                },

                new Course
                {
                    Code = "3MU575",
                    Name = "Master Thesis",
                },

                new Course
                {
                    Code = "3MU677",
                    Name = "Multimedia",
                },

                new Course
                {
                    Code = "3IS102",
                    Name = "Objektorienterad programmering I",
                },

                new Course
                {
                    Code = "3IS202",
                    Name = "Objektorienterad programmering II",
                },

                new Course
                {
                    Code = "2AD339",
                    Name = "Software Engineering",
                },
            };
            courses.ForEach(c => context.Courses.Add(c));
            context.SaveChanges();

            //Populating database with term employment
            foreach (var t in teachers)
            {
                var tteFall = DataGen.GetTeacherTermAvailability(t, HT17, 100);
                var tteSpring = DataGen.GetTeacherTermAvailability(t, VT18, 100);
                context.TeacherTermAvailability.Add(tteFall);
                context.TeacherTermAvailability.Add(tteSpring);
            }
            context.SaveChanges();


            //Populating database with ongoing offerings
            foreach (var c in courses)
            {
                var offering = DataGen.CreateOffering(teachers[DataGen.Rnd.Next(0, teachers.Count)], c, termYears[0],
                    CourseState.Ongoing);
                context.CourseOfferings.Add(offering);
            }
            //Populating the database with planned offerings
            foreach (var c in courses)
            {
                if (DataGen.Rnd.Next(3) > 0)
                {
                    var offering = DataGen.CreateOffering(teachers[DataGen.Rnd.Next(0, teachers.Count)], c, termYears[1],
                    CourseState.Planned);
                    context.CourseOfferings.Add(offering);
                }
            }
            //Populating the database with completed offerings
            foreach (var c in courses)
            {
                
                if (DataGen.Rnd.Next(2) == 0)
                {
                    var offering = DataGen.CreateOffering(teachers[DataGen.Rnd.Next(0, teachers.Count)], c, VT17,
                    CourseState.Completed);
                    context.CourseOfferings.Add(offering);
                }

            }
            context.SaveChanges();
                       
            //Populating database with workloads
            foreach (var c in context.CourseOfferings)
            {
                var workload = DataGen.CreateWorkload(teachers[DataGen.Rnd.Next(0, teachers.Count)], c);
                context.Workloads.Add(workload);
            }            
            context.SaveChanges();


            //Populate the database with academic profiles
            var profiles = new List<AcademicProfile>
            {
                new AcademicProfile
                {
                    Title = AcademicTitle.Professor,
                    TeachingShare = 0.5m,
                    ResearchShare = 0.4m,
                    AdminShare = 0.1m,
                    OtherShare = 0.0m
                },

                new AcademicProfile
                {
                    Title = AcademicTitle.Lektor,
                    TeachingShare = 0.7m,
                    ResearchShare = 0.2m,
                    AdminShare = 0.1m,
                    OtherShare = 0.0m
                },

                new AcademicProfile
                {
                    Title = AcademicTitle.Adjunkt,
                    TeachingShare = 0.8m,
                    ResearchShare = 0.0m,
                    AdminShare = 0.1m,
                    OtherShare = 0.1m
                },

                new AcademicProfile
                {
                    Title = AcademicTitle.Doktorand,
                    TeachingShare = 0.2m,
                    ResearchShare = 0.8m,
                    AdminShare = 0.0m,
                    OtherShare = 0.0m
                },

                new AcademicProfile
                {
                    Title = AcademicTitle.Amanuens,
                    TeachingShare = 1.0m,
                    ResearchShare = 0.0m,
                    AdminShare = 0.0m,
                    OtherShare = 0.0m
                }
            };

            foreach (var profile in profiles)
            {
                context.AcademicProfiles.Add(profile);
            }
            context.SaveChanges();
        }
    }
}