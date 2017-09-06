using System;
namespace GodSpeak
{
	public class ImpactConverter
	{
		public object GetTextForCount(int total)
		{			
			if (total >= 1000)
			{
				var totalDecimal = total / 1000.0;
				var integerNumber = total / 1000;
				var rest = total % 1000;

				var numberOfDecimals = 3 - integerNumber.ToString().ToCharArray().Length;

				if (rest == 0)
				{
					return integerNumber;
				}
				else
				{
					return totalDecimal.ToString("F" + numberOfDecimals) + "k";
				}
			}
			else
			{
				return total;
			}
		}
	}
}
