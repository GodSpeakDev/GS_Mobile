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
		private IShareService _shareService;

		private ObservableCollection<SelectModel<Invite>> _invites;
		public ObservableCollection<SelectModel<Invite>> Invites
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
				return string.Format("{0} {1} Sent {2} Claimed", Invites.Count, Invites.Count > 1 ? "Invites" : "Invite", Invites.Count(x => x.Model.Redeemed));
			}
		}
			
		private MvxCommand<SelectModel<Invite>> _shareCommand;
		public MvxCommand<SelectModel<Invite>> ShareCommand
		{
			get
			{
				return _shareCommand ?? (_shareCommand = new MvxCommand<SelectModel<Invite>>(DoShareCommand));
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

		public InvitesViewModel(IDialogService dialogService, IWebApiService webApi, IShareService shareService) : base(dialogService)
		{
			_webApi = webApi;
			_shareService = shareService;

			Invites = new ObservableCollection<SelectModel<Invite>>();
		}

		public async void Init()
		{
			var response = await _webApi.GetInvites(new GetInvitesRequest());
			if (response.IsSuccess)
			{
				Invites = new ObservableCollection<SelectModel<Invite>>(
					response.Payload.Payload.OrderBy(x => x.Redeemed).Select(x => new SelectModel<Invite>() 
				{
					Model = x,
					Command = ShareCommand
				}));
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

		private void DoShareCommand(SelectModel<Invite> selectModel)
		{
			_shareService.Share(selectModel.Model.Code);
		}
	}
}
