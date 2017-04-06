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
        private IMailService _mailService;

        public ClaimedGiftViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, IMailService mailService) : base (dialogService, hudService, sessionService, webApiService)
        {
            _mailService = mailService;

            SortOptions = new string []
            {
                Text.SortByClaimDate,
                Text.SortByName,
                Text.SortByGiftsGiven
            };
        }

        private int _selectedSortIndex;
        public int SelectedSortIndex {
            get { return _selectedSortIndex; }
            set {
                SetProperty (ref _selectedSortIndex, value);
                SortList ();
            }
        }

        private string [] _sortOptions;
        public string [] SortOptions {
            get { return _sortOptions; }
            set { SetProperty (ref _sortOptions, value); }
        }

        private bool _isVisible;
        public bool IsVisible {
            get { return _isVisible; }
            set { SetProperty (ref _isVisible, value); }
        }

        private ObservableCollection<ItemCommand<AcceptedInvite>> _acceptedInvites;
        public ObservableCollection<ItemCommand<AcceptedInvite>> AcceptedInvites {
            get { return _acceptedInvites; }
            set { SetProperty (ref _acceptedInvites, value); }
        }

        private MvxCommand<AcceptedInvite> _tapInviteCommand;
        public MvxCommand<AcceptedInvite> TapInviteCommand {
            get {
                return _tapInviteCommand ?? (_tapInviteCommand = new MvxCommand<AcceptedInvite> (DoTapInviteCommand));
            }
        }

        public async Task Init ()
        {
            var response = await WebApiService.GetAcceptedInvites (new TokenRequest () { Token = SessionService.GetUser ().Token });

            if (response.IsSuccess) {
                _acceptedInvites = new ObservableCollection<ItemCommand<AcceptedInvite>> (
                    response.Payload.Select (x => new ItemCommand<AcceptedInvite> () {
                        Item = x,
                        TappedCommand = TapInviteCommand
                    }));
                SortList ();
            } else {
                this.HudService.Hide ();
                await HandleResponse (response);
            }
        }

        private void SortList ()
        {
            if (AcceptedInvites == null)
                return;

            var sortOption = SortOptions [SelectedSortIndex];
            if (sortOption == Text.SortByClaimDate) {
                AcceptedInvites = new ObservableCollection<ItemCommand<AcceptedInvite>> (AcceptedInvites.OrderBy (x => x.Item.DateClaimed));
            } else if (sortOption == Text.SortByName) {
                AcceptedInvites = new ObservableCollection<ItemCommand<AcceptedInvite>> (AcceptedInvites.OrderBy (x => x.Item.Title));
            } else if (sortOption == Text.SortByGiftsGiven) {
                AcceptedInvites = new ObservableCollection<ItemCommand<AcceptedInvite>> (AcceptedInvites.OrderBy (x => x.Item.GiftsGiven));
            }
        }

        private void DoTapInviteCommand (AcceptedInvite userModel)
        {
            _mailService.SendMail (to: new string [] { userModel.EmailAddress }, subject: userModel.Subject, body: userModel.Message);
        }
    }
}
