using System;
namespace GodSpeak
{
	public class Address
	{
		public string Street1
		{
			get;
			set;
		}

		public string Street2
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

		public string Zip
		{
			get;
			set;
		}

		public bool IsIncomplete
		{
			get
			{
				return string.IsNullOrEmpty(Street1) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State) || string.IsNullOrEmpty(Zip);
			}

		}

		public Address()
		{
		}

		public bool IsEqualTo(Address address)
		{
			return address.Street1 == Street1 && address.Street2 == Street2 && address.City == City && address.State == State && address.Zip == Zip;
		}
	}
}
