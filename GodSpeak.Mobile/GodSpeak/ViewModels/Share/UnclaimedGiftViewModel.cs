using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using GodSpeak.Services;
using MvvmCross.Plugins.WebBrowser;
using Plugin.InAppBilling;
using GodSpeak.Resources;
using Xamarin.Forms;

namespace GodSpeak
{
	public class UnclaimedGiftViewModel : CustomViewModel
	{
		private IContactService _contactService;
		private IMailService _mailService;

		private DidYouKnowTemplateViewModel _didYouKnowTemplateViewModel;
		public DidYouKnowTemplateViewModel DidYouKnowTemplateViewModel
		{
			get { return _didYouKnowTemplateViewModel; }
		}

		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible; }
			set { SetProperty(ref _isVisible, value); }
		}

		private ObservableCollection<SelectableItem<Contact>> _deviceContacts;
		private ObservableCollection<SelectableItem<Contact>> _contacts;
		public ObservableCollection<SelectableItem<Contact>> Contacts
		{
			get { return _contacts; }
			set { SetProperty(ref _contacts, value); }
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

		private MvxCommand _inviteThemCommand;
		public MvxCommand InviteThemCommand
		{
			get
			{
				return _inviteThemCommand ?? (_inviteThemCommand = new MvxCommand(DoInviteThemCommand));
			}
		}

		private string _searchText;
		public string SearchText
		{
			get { return _searchText; }
			set
			{
				SetProperty(ref _searchText, value);
				RefreshContactList();
			}
		}

		public UnclaimedGiftViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IContactService contactService, IMailService mailService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{
			_contactService = contactService;
			_mailService = mailService;
		}

		public async Task Init(bool comesFromRegisterFlow)
		{
			_didYouKnowTemplateViewModel = new DidYouKnowTemplateViewModel(DialogService, HudService, SessionService, WebApiService, SettingsService);
			await _didYouKnowTemplateViewModel.Init();

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
				this.HudService.Hide();
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
			var contacts = _deviceContacts.Where(x => (
				string.IsNullOrEmpty(SearchText)
				||
				(x.Item.FirstName != null && x.Item.FirstName.ToLower().Contains(SearchText.ToLower())) ||
				x.Item.LastName != null && x.Item.LastName.ToLower().Contains(SearchText.ToLower())));

			Contacts = new ObservableCollection<SelectableItem<Contact>>(contacts);
		}

		private void DoInviteThemCommand()
		{
			if (HasEmailSelected)
			{
				var selectedEmails = _deviceContacts.Where(x => x.IsEnabled).Select(x => x.Item.EmailAddresses.First().Address).ToArray();
				this.ShowViewModel<EmailComposerViewModel>(new { toAddresses = string.Join(",", selectedEmails) });
			}
		}
	}
}
