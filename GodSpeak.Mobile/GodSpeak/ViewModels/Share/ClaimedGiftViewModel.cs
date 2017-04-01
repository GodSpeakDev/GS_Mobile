using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GodSpeak.Resources;
using MvvmCross.Core.ViewModels;
using GodSpeak.Services;

namespace GodSpeak
{
	public class ClaimedGiftViewModel : CustomViewModel
	{		
		private IShareService _shareService;

		public ClaimedGiftViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, IShareService shareService) : base(dialogService, hudService, sessionService, webApiService)
		{			
			_shareService = shareService;

			SortOptions = new string[] 
			{
				Text.SortByClaimDate
			};
		}

		private int _selectedSortIndex;
		public int SelectedSortIndex
		{
			get { return _selectedSortIndex;}
			set { SetProperty(ref _selectedSortIndex, value); }
		}

		private string[] _sortOptions;
		public string[] SortOptions
		{
			get { return _sortOptions; }
			set { SetProperty(ref _sortOptions, value); }
		}

		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible;}
			set { SetProperty(ref _isVisible, value);}
		}

		private ObservableCollection<ItemCommand<AcceptedInvite>> _acceptedInvites;
		public ObservableCollection<ItemCommand<AcceptedInvite>> AcceptedInvites
		{
			get { return _acceptedInvites;}
			set { SetProperty(ref _acceptedInvites, value);}
		}

		private MvxCommand<AcceptedInvite> _tapInviteCommand;
		public MvxCommand<AcceptedInvite> TapInviteCommand
		{
			get
			{
				return _tapInviteCommand ?? (_tapInviteCommand = new MvxCommand<AcceptedInvite>(DoTapInviteCommand));
			}
		}

		public async Task Init()
		{			
			var response = await WebApiService.GetAcceptedInvites(new TokenRequest() { Token = SessionService.GetUser().Token });						

			if (response.IsSuccess)
			{
				AcceptedInvites = new ObservableCollection<ItemCommand<AcceptedInvite>>(
					response.Payload.Select(x => new ItemCommand<AcceptedInvite>() 
					{
						Item = x,
						TappedCommand = TapInviteCommand
					}));
			}
			else
			{
				this.HudService.Hide();
				await HandleResponse(response);
			}
		}

		private async void DoTapInviteCommand(AcceptedInvite userModel)
		{
			var userModel1 = userModel;
			await this.DialogService.ShowAlert("In Development", "In Development");
		}
	}
}
