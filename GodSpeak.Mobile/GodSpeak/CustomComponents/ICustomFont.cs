using System;

namespace GodSpeak
{
	public interface ICustomFont
	{
		GodSpeak.FontWeight FontWeight
		{
			get; 
			set;
		}

		double FontSize
		{
			get;
			set;
		}
	}
}
