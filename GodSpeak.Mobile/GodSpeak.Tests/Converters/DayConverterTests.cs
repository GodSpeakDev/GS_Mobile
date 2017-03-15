using System;
using NUnit.Framework;
using GodSpeak;

namespace GodSpeak.Tests
{
	[TestFixture()]
	public class DayConverterTests
	{
		private DayConverter _dayConverter;

		[SetUp]
		public void SetUp()
		{
			_dayConverter = new DayConverter();
		}

		private string Convert(DateTime date)
		{
			return (string) _dayConverter.Convert(date, null, null, null);
		}

		[Test()]
		public void If_Date_Is_Today_Then_SHOULD_Show_Today()
		{
			Assert.AreEqual(GodSpeak.Resources.Text.Today, Convert(DateTime.Now));
		}

		[Test()]
		public void If_Date_Is_Yesterday_Then_SHOULD_Show_Yesterday()
		{
			Assert.AreEqual(GodSpeak.Resources.Text.Yesterday, Convert(DateTime.Now.AddDays(-1)));
		}

		[Test()]
		public void If_Date_Difference_Is_Less_Than_Seven_Days_SHOULD_Show_Weekday_Name()
		{
			Assert.AreEqual(DateTime.Now.AddDays(-2).ToString("dddd"), Convert(DateTime.Now.AddDays(-2)));
		}

		[Test()]
		public void If_Date_Difference_Is_Greater_Than_Seven_Days_SHOULD_Show_Date()
		{
			Assert.AreEqual(DateTime.Now.AddDays(-7).ToString("d"), Convert(DateTime.Now.AddDays(-7)));
		}
	}
}
