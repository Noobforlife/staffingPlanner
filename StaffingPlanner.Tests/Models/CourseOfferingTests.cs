using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffingPlanner.Models;

namespace StaffingPlanner.Tests.Models
{
	[TestClass]
	public class CourseOfferingTests
	{
		[TestMethod]
		public void TruncatedName_EmptyString_ShouldNotTruncate()
		{
			var course = new Course
			{
				Code = "",
				Name = ""
			};

			Assert.AreEqual("", course.TruncatedName);
		}

		[TestMethod]
		public void TruncatedName_NameShorterThanTruncateLimit_ShouldNotTruncate()
		{
			var course = new Course
			{
				Code = "",
				Name = "Agile"
			};

			Assert.AreEqual("Agile", course.TruncatedName);
		}

		[TestMethod]
		public void TruncatedName_NameLengthAtTruncateLimit_ShouldNotTruncate()
		{
			var course = new Course
			{
				Code = "",
				Name = "Dotnet-programmering"
			};

			Assert.AreEqual("Dotnet-programmering", course.TruncatedName);
		}


		[TestMethod]
		public void TruncatedName_NameLongerThanTruncateLimit_ShouldNotTruncate()
		{
			var course = new Course
			{
				Code = "",
				Name = "Artificial intelligence"
			};

			Assert.AreEqual("Artificial intellig...", course.TruncatedName);
		}

		[TestMethod]
		public void Equals_null_false()
		{
			var offering = new CourseOffering();
			Assert.IsFalse(offering.Equals(null));
		}

		[TestMethod]
		public void Equals_NotSameId_false()
		{
			var offering1 = new CourseOffering {Id = Guid.NewGuid()};
			var offering2 = new CourseOffering { Id = Guid.NewGuid() };
			Assert.IsFalse(offering1.Equals(offering2));
		}

		[TestMethod]
		public void Equals_SameId_true()
		{
			var id = Guid.NewGuid();
			var offering1 = new CourseOffering {Id = id};
			var offering2 = new CourseOffering { Id = id };
			Assert.IsTrue(offering1.Equals(offering2));
		}

		// Status
		// warning, success, danger, edge cases between these
		// TODO: Test when actual warnings are implemented
		// Use CourseOffering offering = new MockCourseOffering {AllocatedHours = a value that will allow you to test the different statuses};
	}

	public class MockCourseOffering : CourseOffering
	{
		public new int AllocatedHours { get; set; }
	}
}
