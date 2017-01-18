using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class User
	{
		public User()
		{
			SelectedCategories = new List<MessageCategory>();
			DayOfWeekSettings = new List<DayOfWeekSettings>();
		}

		public string Token
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public decimal Credits
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string PhotoUrl
		{
			get;
			set;
		}

		public List<MessageCategory> SelectedCategories
		{
			get;
			set;
		}

		public List<DayOfWeekSettings> DayOfWeekSettings
		{
			get;
			set;
		}
	}
}
