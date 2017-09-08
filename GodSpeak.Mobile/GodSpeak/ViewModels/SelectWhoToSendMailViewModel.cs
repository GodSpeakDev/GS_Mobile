using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using MvvmCross.Core;
using MvvmCross.Forms;
using MvvmCross.Platform;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GodSpeak.Services;

namespace GodSpeak
{
	public class SelectWhoToSendMailViewModel : CustomViewModel
	{
		private IContactService _contactService;
		private IMailService _mailService;

		private MvxCommand _composeEmailCommand;
		public MvxCommand ComposeEmailCommand
		{
			get
			{
				return _composeEmailCommand ?? (_composeEmailCommand = new MvxCommand(DoComposeEmailCommand));
			}
		}

		private string _searchText;
		public string SearchText
		{
			get { return _searchText;}
			set { 
				SetProperty(ref _searchText, value);
				RefreshContactList();
			}
		}

		private bool _hasEmailSelected;
		public bool HasEmailSelected
		{
			get { return _hasEmailSelected; }
			set
			{
				SetProperty(ref _hasEmailSelected, value);
			}
		}

		private ObservableCollection<SelectableItem<Contact>> _contacts;
		public ObservableCollection<SelectableItem<Contact>> Contacts
		{
			get { return _contacts;}
			set { SetProperty(ref _contacts, value);}
		}

		private ObservableCollection<SelectableItem<Contact>> _deviceContacts;

		public SelectWhoToSendMailViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IContactService contactService, IMailService mailService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{
			_contactService = contactService;
			_mailService = mailService;
		}

		public async void Init() 
		{
			if (await _contactService.CanAccessContacts())
			{
				_contactService.GetAllContacts((List<Contact> contacts) =>
				{
					_deviceContacts = new ObservableCollection<SelectableItem<Contact>>(contacts.Where(x => x.EmailAddresses.Count > 0).OrderBy(x => x.FirstName).Select(x => new SelectableItem<Contact>()
					{
						Item = x,
					}));

					foreach (var contact in _deviceContacts)
					{
						contact.PropertyChanged -= UpdatedItem;
						contact.PropertyChanged += UpdatedItem;
					}

					RefreshContactList();
				});
			}
			else
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.ContactAccessDenied);
			}
		}

		private void UpdatedItem(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsEnabled")
			{
				HasEmailSelected = _deviceContacts.Any(x => x.IsEnabled);
			}
		}

		private void RefreshContactList()
		{
			if (CancellationToken.IsCancellationRequested)
			{
				return;
			}

			if (_deviceContacts == null)
			{
				Contacts = new ObservableCollection<SelectableItem<Contact>>();
			}
			else
			{
				var contacts = _deviceContacts.Where(x => (
					string.IsNullOrEmpty(SearchText)
					||
					(x.Item.FirstName != null && x.Item.FirstName.ToLower().Contains(SearchText.ToLower())) ||
					x.Item.LastName != null && x.Item.LastName.ToLower().Contains(SearchText.ToLower())));

				Contacts = new ObservableCollection<SelectableItem<Contact>>(contacts);
			}
		}

		private async void DoComposeEmailCommand()
		{
			if (HasEmailSelected)
			{
				var selectedContacsMail = _deviceContacts.Where(x => x.IsEnabled).Select(x => x.Item.EmailAddresses.First().Address);
				//var selectedContacsMail = new string[] {"paulo.ortins@gmail.com"};
				var body = string.Format(Text.ShareEmailBody, (await SessionService.GetUser()).FirstName);
				_mailService.SendMail(to: selectedContacsMail.ToArray(), subject: Text.ShareEmailSubject, body: body);
			}	
		}
	}
}
