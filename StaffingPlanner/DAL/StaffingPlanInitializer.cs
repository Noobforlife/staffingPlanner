using System.Collections.Generic;
using StaffingPlanner.Models;
using System;

namespace StaffingPlanner.DAL
{
	public class StaffingPlanInitializer : System.Data.Entity.DropCreateDatabaseAlways<StaffingPlanContext>
	{
		protected override void Seed(StaffingPlanContext context)
		{
			var courses = new List<Course> {};

			courses.ForEach(c => context.Courses.Add(c));
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
                    DirectorOfStudies = true
                },

                new Teacher()
                {
                    Name = "Andreas Hamfeldt",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "610427-1541",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Mats Edenius",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "610427-1541",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Pär Ågerfalk",
                    AcademicTitle = AcademicTitle.Professor,
                    PersonalNumber = "740905-2886",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Anneli Edman",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "560129-7352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Barbro Funseth",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "560129-7352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Claes Thorén",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Franck Tétard",
                    AcademicTitle = AcademicTitle.Adjunkt,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Anton Backe",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "920610-8361",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Christer Stuxberg",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Görkem Pacaci",
                    AcademicTitle = AcademicTitle.Lektor,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Mustafa Mudassir Imran",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "740905-2886",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Christopher Ohkravi",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Asma Rafiq",
                    AcademicTitle = AcademicTitle.Doktorand,
                    PersonalNumber = "821111-8352",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Sofie Roos",
                    AcademicTitle = AcademicTitle.Amanuens,
                    PersonalNumber = "920610-8361",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },

                new Teacher()
                {
                    Name = "Daniel Wallman",
                    AcademicTitle = AcademicTitle.Amanuens,
                    PersonalNumber = "920610-8361",
                    Email = "",
                    Id = Guid.NewGuid(),
                    DirectorOfStudies = false
                },


            };

			teachers.ForEach(t => context.Teachers.Add(t));
			context.SaveChanges();

			var workloads = new List<TeacherCourseWorkload> { };

			workloads.ForEach(w => context.Workloads.Add(w));
			context.SaveChanges();
		}
	}
}