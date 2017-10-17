using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffingPlanner.Models;

namespace StaffingPlanner.Tests.Models
{
	[TestClass]
	public class TermYearTests
	{
		[TestMethod]
		public void ToString_Fall2017_HTDash17()
		{
			var termYear = new TermYear
			{
				Term = Term.Fall,
				Year = 2017
			};

			Assert.AreEqual("HT-17", termYear.ToString());
		}

		[TestMethod]
		public void ToString_Spring1999_VTDash99()
		{
			var termYear = new TermYear
			{
				Term = Term.Spring,
				Year = 1999
			};

			Assert.AreEqual("VT-99", termYear.ToString());
		}
	}
}
