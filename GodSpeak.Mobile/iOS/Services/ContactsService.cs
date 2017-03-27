using System;
using System.Collections.Generic;
using GodSpeak;
using AddressBook;
using Foundation;
using System.Linq;
using UIKit;
using AddressBookUI;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GodSpeak.iOS
{
	public class ContactsService : IContactService
	{
		public static UINavigationController NavController;

		public ContactsService()
		{
		}

		public void LaunchContactBrowser(Action<Contact> onSelectedAction)
		{
			using (var addressBook = new ABAddressBook())
			{

				try
				{
					if (addressBook != null)
					{
						addressBook.RequestAccess((bool haveAccess, NSError e) =>
						{
							if (haveAccess)
							{
								NavController.InvokeOnMainThread(() =>
								{
									var peoplePicker = new ABPeoplePickerNavigationController();
									peoplePicker.SelectPerson2 += async (object picker, ABPeoplePickerSelectPerson2EventArgs args) => onSelectedAction(await ParseContact(args.Person));
									NavController.PresentModalViewController(peoplePicker, true);
								});


							}
							else
							{
								//new UIAlertView ("", "Ok, we understand if you do not want StyleStaytion accessing your contacts. To enable StyleStaytion to access the Address Book in the future you will need to update your device settings.", null, "Ok", null).Show ();
							}
						});
					}
				}
				catch
				{
					DisplayAccessDenied();
				}
			}
		}

		public void DisplayAccessDenied()
		{
			var alert = new UIAlertController()
			{
				Title = "",
				Message = "Sorry, you have previously denied StyleSaytion access to your contacts."
			};

			var settingsAction = UIAlertAction.Create("Fix it!", UIAlertActionStyle.Default, (obj) =>
			{
				var url = new NSUrl(UIApplication.OpenSettingsUrlString);
				if (url != null)
				{
					UIApplication.SharedApplication.OpenUrl(url);
				}
			});

			alert.AddAction(settingsAction);

			var cancelAction = UIAlertAction.Create("Nevermind", UIAlertActionStyle.Cancel, null);
			alert.AddAction(cancelAction);

			var window = UIApplication.SharedApplication.KeyWindow;
			var vc = window.RootViewController;
			if (vc.GetType() == typeof(UINavigationController))
			{
				if ((vc as UINavigationController).TopViewController != null)
				{
					vc = (vc as UINavigationController).TopViewController;
					vc.PresentViewController(alert, true, null);
				}

			}
			else
			{

				while (vc.PresentedViewController != null)
				{
					vc = vc.PresentedViewController;

				}

				vc.InvokeOnMainThread(() =>
				{
					vc.PresentViewController(alert, true, null);
				});

			}
		}

		public void GetAllContacts(Action<List<Contact>> resultAction)
		{
			try
			{
				var addressBook = new ABAddressBook();

				addressBook.RequestAccess((bool haveAccess, NSError e) =>
				{
					if (haveAccess)
					{

						var contacts = addressBook.GetPeople();

						resultAction(contacts.Where(c => !(string.IsNullOrEmpty(c.FirstName) && string.IsNullOrEmpty(c.LastName) && string.IsNullOrEmpty(c.Organization))).Select(c => new Contact()
						{
							Id = c.Id,
							FirstName = c.FirstName,
							LastName = c.LastName,
							Organization = c.Organization,
							EmailAddresses = c.GetEmails().Select(x => new EmailAddress() 
							{
								Address = x.Value,
								Label = x.Label
							}).ToList()
						}).ToList());
					}
					else
					{
						resultAction(new List<Contact>());
					}
				});
			}
			catch (Exception ex)
			{
				resultAction(new List<Contact>());
			}
		}

		public async Task<Contact> ParseContact(object contact)
		{

			var submittedPerson = contact as Contact;

			var abperson = (submittedPerson != null) ? new ABAddressBook().GetPeople().First(p => p.Id == submittedPerson.Id) : (ABPerson)contact;


			var person = new Contact();
			person.Id = abperson.Id;
			person.FirstName = abperson.FirstName;
			person.LastName = abperson.LastName;
			person.Organization = abperson.Organization;

			await ParsePhoneNumber(abperson, person);

			await ParseEmail(abperson, person);

			await ParseAddress(abperson, person);

			return person;
		}

		protected async Task ParseAddress(ABPerson contact, Contact person)
		{
			var addresses = contact.GetAllAddresses();
			if (addresses.Count > 0)
			{
				//nint selectedAddressIndex = (addresses.Count > 1) ? await ShowAlert ("Select Address", "Which address should we use?", addresses.Select (a => a.Value.Street).ToArray ()) : 0;
				nint selectedAddressIndex = 0;
				var address = addresses[selectedAddressIndex].Value;
				person.Address = new Address()
				{
					Street1 = address.Street,
					City = address.City,
					State = address.State,
					Zip = address.Zip
				};
			}
		}

		protected string cleanNSString(string ns)
		{
			//_$!<Home>!$_
			try
			{
				var str = ns.ToString().Replace("_$!<", string.Empty).Replace(">!$_", string.Empty);
				return str;
			}
			catch
			{

			}
			return ns;
		}

		protected async Task ParseEmail(ABPerson contact, Contact person)
		{
			var emails = contact.GetEmails();
			person.EmailAddresses = new List<EmailAddress>();
			if (emails.Count > 0)
			{
				foreach (ABMultiValueEntry<string> item in emails)
				{

					person.EmailAddresses.Add(new EmailAddress()
					{
						Label = cleanNSString(item.Label),
						Address = cleanNSString(item.Value)
					});
				}

			}
			if (person.EmailAddresses.Any(e => !string.IsNullOrEmpty(e.Label)))
			{
				var personalEmail = person.EmailAddresses.FirstOrDefault(e => e.Label?.ToLower() == "home" || e.Label?.ToLower() == "personal");
				if (personalEmail != null)
				{
					person.EmailAddresses.Remove(personalEmail);
					person.EmailAddresses.Insert(0, personalEmail);
				}
			}

		}

		protected async Task ParsePhoneNumber(ABPerson contact, Contact person)
		{
			var phoneNumbers = contact.GetPhones();
			person.PhoneNumbers = new List<PhoneNumber>();
			if (phoneNumbers.Count > 0)
			{
				foreach (ABMultiValueEntry<string> item in phoneNumbers)
				{

					person.PhoneNumbers.Add(new PhoneNumber()
					{
						Label = cleanNSString(item.Label),
						Number = cleanNSString(item.Value)
					});
				}

			}
			if (person.PhoneNumbers.Any(e => !string.IsNullOrEmpty(e.Label)))
			{
				var mobileNumber = person.PhoneNumbers.FirstOrDefault(p => p.Label?.ToLower() == "mobile" || p.Label?.ToLower() == "cell");
				if (mobileNumber != null)
				{
					person.PhoneNumbers.Remove(mobileNumber);
					person.PhoneNumbers.Insert(0, mobileNumber);
				}
			}
		}

		public static Task<nint> ShowAlert(string title,
										   string message,
										   params string[] buttons)
		{
			var tcs = new TaskCompletionSource<nint>();
			var alert = new UIAlertView
			{
				Title = title,
				Message = message
			};
			foreach (var button in buttons)
				alert.AddButton(button);
			alert.Clicked += (s, e) => tcs.TrySetResult(e.ButtonIndex);
			alert.Show();
			return tcs.Task;
		}

		public Task<bool> CanAccessContacts()
		{
			return Task.Run(() =>
			{
				var t = new TaskCompletionSource<bool>();

				using (var addressBook = new ABAddressBook())
				{
					try
					{
						if (addressBook != null)
						{
							addressBook.RequestAccess((bool haveAccess, NSError e) =>
							{
								t.TrySetResult(haveAccess);
							});
						}
					}
					catch
					{
						t.TrySetResult(false);
					}

				}

				return t.Task;
			});
		}
	}
}
