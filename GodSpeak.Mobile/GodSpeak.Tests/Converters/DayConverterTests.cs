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
		public void If_Date_Difference_Is_Less_Than_Seven_Days_Should_Show_Weekday_Name()
		{
			Assert.AreEqual(DateTime.Now.ToString("dddd"), Convert(DateTime.Now));
		}

		[Test()]
		public void If_Date_Difference_Is_Greater_Than_Seven_Days_Should_Show_Weekday_Name()
		{
			Assert.AreEqual(DateTime.Now.AddDays(-7).ToString("d"), Convert(DateTime.Now.AddDays(-7)));
		}
	}
}
