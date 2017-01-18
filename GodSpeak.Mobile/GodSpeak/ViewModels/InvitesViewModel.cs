using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class InvitesViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private ObservableCollection<Invite> _invites;
		public ObservableCollection<Invite> Invites
		{
			get { return _invites; }
			set { 
				SetProperty(ref _invites, value);
				RaisePropertyChanged(nameof(InviteSummary));
			}
		}

		public string InviteSummary
		{
			get 
			{
				return string.Format("{0} {1} Sent {2} Claimed", Invites.Count, Invites.Count > 1 ? "Invites" : "Invite", Invites.Count(x => x.Redeemed));
			}
		}

		private MvxCommand _purchaseMoreInviteCommand;
		public MvxCommand PurchaseMoreInviteCommand
		{
			get
			{
				return _purchaseMoreInviteCommand ?? (_purchaseMoreInviteCommand = new MvxCommand(DoPurchaseMoreInviteCommand));
			}
		}

		public InvitesViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;

			Invites = new ObservableCollection<Invite>();
		}

		public async void Init()
		{
			var response = await _webApi.GetInvites(new GetInvitesRequest());
			if (response.IsSuccess)
			{
				Invites = new ObservableCollection<Invite>(response.Content.Payload.OrderBy(x => x.Redeemed));
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private void DoPurchaseMoreInviteCommand()
		{
			this.ShowViewModel<PurchaseCreditViewModel>();
		}
	}
}
