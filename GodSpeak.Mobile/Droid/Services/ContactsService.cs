using System;
using System.Threading.Tasks;
using GodSpeak;
using Android.OS;
using Android.Content;
using Android.Database;
using Android.App;
using System.Linq;
using System.Diagnostics;
using Android.Provider;
using Android.Widget;
using System.Collections.Generic;
using Xamarin.Forms;
using Android;
using Android.Content.PM;
using Plugin.Contacts;

namespace GodSpeak.Droid
{
	public class ContactsService : IContactService
	{
		private TaskCompletionSource<bool> _tcs;
		private readonly AddressBook _contactsAdressBook;

		public ContactsService()
		{
			_contactsAdressBook = new AddressBook(Forms.Context);
		}

		public void GetAllContacts(Action<List<Contact>> resultAction)
		{
			var addressBookContacts = _contactsAdressBook.ToList();
			var contacts = new List<Contact>();

			foreach (var phoneContact in addressBookContacts)
			{
				var contact = new Contact();
				contact.FirstName = phoneContact.FirstName;
				contact.LastName = phoneContact.LastName;
				contact.EmailAddresses = phoneContact.Emails.Select(x => new EmailAddress()
				{
					Address = x.Address,
					Label = x.Label
				}).ToList();

				contacts.Add(contact);
			}

			resultAction(contacts);
		}

		public void OnRequestPermissionsResult(bool isGranted)
		{
			_tcs?.SetResult(isGranted);
		}

		public Task<bool> CanAccessContacts()
		{
			if ((int)Build.VERSION.SdkInt < 23 ||
				Forms.Context.CheckSelfPermission(Manifest.Permission.ReadContacts) == (int)Permission.Granted)
			{
				return Task.FromResult(true);
			}

			if (_tcs != null && !_tcs.Task.IsCompleted)
			{
				_tcs.SetCanceled();
				_tcs = null;
			}

			_tcs = new TaskCompletionSource<bool>();

			(Forms.Context.ApplicationContext as Activity).RequestPermissions(new[] { Manifest.Permission.ReadContacts }, MainActivity.RequestReadContacts);

			return _tcs.Task;
		}
	}
}
