using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaffingPlanner.Models;

namespace StaffingPlanner.Tests.Models
{
	[TestClass]
	public class CourseEnumsTests
	{
		[TestMethod]
		public void PeriodToString_P1_P1()
		{
			Assert.AreEqual("P1", EnumToString.PeriodToString(Period.P1));
		}

		[TestMethod]
		public void PeriodToString_P2_P2()
		{
			Assert.AreEqual("P2", EnumToString.PeriodToString(Period.P2));
		}

		[TestMethod]
		public void PeriodToString_P3_P3()
		{
			Assert.AreEqual("P3", EnumToString.PeriodToString(Period.P3));
		}

		[TestMethod]
		public void PeriodToString_P4_P4()
		{
			Assert.AreEqual("P4", EnumToString.PeriodToString(Period.P4));
		}

		[TestMethod]
		public void PeriodToString_P1P2_P1Dash2()
		{
			Assert.AreEqual("P1-2", EnumToString.PeriodToString(Period.P1P2));
		}

		[TestMethod]
		public void PeriodToString_P3P4_P3Dash4()
		{
			Assert.AreEqual("P3-4", EnumToString.PeriodToString(Period.P3P4));
		}

		[TestMethod]
		public void PeriodToString_AllPeriods_P1Dash4()
		{
			Assert.AreEqual("P1-4", EnumToString.PeriodToString(Period.AllPeriods));
		}
	}
}
