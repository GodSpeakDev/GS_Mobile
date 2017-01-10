using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class LoginResponse
	{
		public LoginResponse()
		{
			SelectedCategories = new List<Category>();
		}

		public string Token
		{
			get;
			set;
		}

		public long Id
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

		public string ProfilePhoto
		{
			get;
			set;
		}

		public MessageConfig MessageConfig
		{
			get;
			set;
		}

		public List<Category> SelectedCategories
		{
			get;
			set;
		}
	}
}
