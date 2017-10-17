using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffingPlanner.Models;

namespace StaffingPlanner.Tests.Models
{
	[TestClass]
	public class CourseTests
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
	}
}
