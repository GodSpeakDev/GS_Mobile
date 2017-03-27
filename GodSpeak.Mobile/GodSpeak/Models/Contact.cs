using System;
using System.Collections.Generic;
using System.Linq;

namespace GodSpeak
{
	public class Contact
	{
		public int Id
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

		public string Organization
		{
			get;
			set;
		}

		public List<PhoneNumber> PhoneNumbers
		{
			get;
			set;
		}

		public List<EmailAddress> EmailAddresses
		{
			get;
			set;
		}

		public string Description
		{
			get { return string.Format("{0} {1} - {2}", FirstName, LastName, EmailAddresses.FirstOrDefault().Address);	}
		}

		public Address Address
		{
			get;
			set;
		}

		public Contact()
		{
			Address = new Address();
			PhoneNumbers = new List<PhoneNumber>();
			EmailAddresses = new List<EmailAddress>();
		}
	}
}
