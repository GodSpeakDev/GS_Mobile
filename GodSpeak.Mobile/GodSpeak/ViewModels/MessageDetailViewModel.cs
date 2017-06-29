using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using Xamarin.Forms;
using GodSpeak.Services;
using GodSpeak.Resources;

namespace GodSpeak
{
    public class MessageDetailViewModel : CustomViewModel
    {
        private IShareService _shareService;
		private IMessageService _messageService;

        private Message _message;
        public Message Message {
            get { return _message; }
            set {
                SetProperty (ref _message, value);
            }
        }

        private string _author;
        public string Author {
            get { return _author; }
            set { SetProperty (ref _author, value); }
        }

        private Color [] _gradientColors;
        public Color [] GradientColors {
            get { return _gradientColors; }
            set { SetProperty (ref _gradientColors, value); }
        }

        private bool _beforeVerseSelected;
        public bool BeforeVerseSelected {
            get { return _beforeVerseSelected; }
            set {
                SetProperty (ref _beforeVerseSelected, value);
                if (_beforeVerseSelected) {
                    CurrentVerseSelected = false;
                    AfterVerseSelected = false;
                    GradientColors = new Color []
                    {ColorHelper.IosDarkBlueGradient, ColorHelper.IosLightBlueGradient};
                    Author = Message?.PreviousVerse.Title;
                }
            }
        }

        private MvxCommand _selectBeforeVerseCommand;
        public MvxCommand SelectBeforeVerseCommand {
            get {
                return _selectBeforeVerseCommand ?? (_selectBeforeVerseCommand = new MvxCommand (() => BeforeVerseSelected = true));
            }
        }

        private bool _currentVerseSelected;
        public bool CurrentVerseSelected {
            get { return _currentVerseSelected; }
            set {
                SetProperty (ref _currentVerseSelected, value);
                if (_currentVerseSelected) {
                    BeforeVerseSelected = false;
                    AfterVerseSelected = false;
                    GradientColors = new Color []
                    {ColorHelper.IosLightBlueGradient, ColorHelper.IosDarkBlueGradient, ColorHelper.IosLightBlueGradient};
                    Author = Message?.Verse.Title;
                }
            }
        }

        private MvxCommand _selectCurrentVerseCommand;
        public MvxCommand SelectCurrentVerseCommand {
            get {
                return _selectCurrentVerseCommand ?? (_selectCurrentVerseCommand = new MvxCommand (() => CurrentVerseSelected = true));
            }
        }

        private bool _afterVerseSelected;
        public bool AfterVerseSelected {
            get { return _afterVerseSelected; }
            set {
                SetProperty (ref _afterVerseSelected, value);
                if (_afterVerseSelected) {
                    BeforeVerseSelected = false;
                    CurrentVerseSelected = false;
                    GradientColors = new Color []
                    {ColorHelper.IosLightBlueGradient, ColorHelper.IosDarkBlueGradient};
                    Author = Message?.NextVerse.Title;
                }
            }
        }

        private MvxCommand _selectAfterVerseCommand;
        public MvxCommand SelectAfterVerseCommand {
            get {
                return _selectAfterVerseCommand ?? (_selectAfterVerseCommand = new MvxCommand (() => AfterVerseSelected = true));
            }
        }

        public MessageDetailViewModel (
            IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService, IMessageService messageService) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            _shareService = shareService;
			_messageService = messageService;
        }

        private MvxCommand _shareCommand;
        public MvxCommand ShareCommand {
            get {
                return _shareCommand ?? (_shareCommand = new MvxCommand (DoShareCommand));
            }
        }

        public async void Init (string messageId)
        {
            CurrentVerseSelected = true;

            var message = await _messageService.GetSingleMessage (new Guid (messageId));

            if (message != null) 
			{
                Message = message;
                Author = Message.Verse.Title;

				if (!SettingsService.DeliveredVerseCodes.Contains(Message.Verse.Title))
				{
					var deliverResponse = await WebApiService.RecordMessageDelivered(new RecordMessageDeliveredRequest
					{
						VerseCode = Message.Verse.Title,
						DateDelivered = DateTime.Now
					});
				}
            } 
			else 
			{
                //await HandleResponse (messageResponse);
            }
        }

        private void DoShareCommand ()
        {
			var formattedVerse = Message.Verse.Text.FormatVerse();
			_shareService.Share (string.Format ("{0} - {1} \n\n{2}", formattedVerse, Message.Verse.Title, Text.GodSpeakWebSite));
        }
    }
}
