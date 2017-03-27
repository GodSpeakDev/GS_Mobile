﻿using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using Xamarin.Forms;

namespace GodSpeak
{
	public class MessageDetailViewModel : CustomViewModel
	{
		private IShareService _shareService;
		private IWebApiService _webApiService;

		private Message _message;
		public Message Message
		{
			get { return _message;}
			set 
			{
				SetProperty(ref _message, value);
			}
		}

		private string _author;
		public string Author
		{
			get { return _author;}
			set { SetProperty(ref _author, value);}
		}

		private Color[] _gradientColors;
		public Color[] GradientColors
		{
			get { return _gradientColors;}
			set { SetProperty(ref _gradientColors, value);}
		}

		private bool _beforeVerseSelected;
		public bool BeforeVerseSelected
		{
			get { return _beforeVerseSelected;}
			set { 
				SetProperty(ref _beforeVerseSelected, value);
				if (_beforeVerseSelected)
				{
					CurrentVerseSelected = false;
					AfterVerseSelected = false;
					GradientColors = new Color[] 
					{ColorHelper.IosDarkBlueGradient, ColorHelper.IosLightBlueGradient};
					Author = Message?.BeforeAuthor;
				}
			}
		}

		private MvxCommand _selectBeforeVerseCommand;
		public MvxCommand SelectBeforeVerseCommand
		{
			get
			{
				return _selectBeforeVerseCommand ?? (_selectBeforeVerseCommand = new MvxCommand(() => BeforeVerseSelected = true));
			}
		}

		private bool _currentVerseSelected;
		public bool CurrentVerseSelected
		{
			get { return _currentVerseSelected; }
			set 
			{ 
				SetProperty(ref _currentVerseSelected, value); 
				if (_currentVerseSelected)
				{
					BeforeVerseSelected = false;
					AfterVerseSelected = false;
					GradientColors = new Color[]
					{ColorHelper.IosLightBlueGradient, ColorHelper.IosDarkBlueGradient, ColorHelper.IosLightBlueGradient};
					Author = Message?.Author;
				}
			}
		}

		private MvxCommand _selectCurrentVerseCommand;
		public MvxCommand SelectCurrentVerseCommand
		{
			get
			{
				return _selectCurrentVerseCommand ?? (_selectCurrentVerseCommand = new MvxCommand(() => CurrentVerseSelected = true));
			}
		}

		private bool _afterVerseSelected;
		public bool AfterVerseSelected
		{
			get { return _afterVerseSelected; }
			set 
			{ 
				SetProperty(ref _afterVerseSelected, value); 
				if (_afterVerseSelected)
				{
					BeforeVerseSelected = false;
					CurrentVerseSelected = false;
					GradientColors = new Color[]
					{ColorHelper.IosLightBlueGradient, ColorHelper.IosDarkBlueGradient};
					Author = Message?.AfterAuthor;
				}
			}
		}

		private MvxCommand _selectAfterVerseCommand;
		public MvxCommand SelectAfterVerseCommand
		{
			get
			{
				return _selectAfterVerseCommand ?? (_selectAfterVerseCommand = new MvxCommand(() => AfterVerseSelected = true));
			}
		}

		public MessageDetailViewModel(
			IDialogService dialogService,
			IWebApiService webApiService,
			IShareService shareService) : base(dialogService)
		{
			_shareService = shareService;
			_webApiService = webApiService;
		}

		private MvxCommand _shareCommand;
		public MvxCommand ShareCommand
		{
			get
			{
				return _shareCommand ?? (_shareCommand = new MvxCommand(DoShareCommand));
			}
		}

		public async void Init(string messageId)
		{		
			CurrentVerseSelected = true;	

			var messageResponse = await _webApiService.GetMessage(new GetMessageRequest() 
			{
				MessageId = new Guid(messageId)
			});

			if (messageResponse.IsSuccess)
			{
				Message = messageResponse.Payload.Payload;
				Author = Message.Author;
			}
			else
			{
				await HandleResponse(messageResponse);
			}
		}

		private void DoShareCommand()
		{
			_shareService.Share(string.Format("{0} - {1}", Message.Text, Message.Author));
		}
	}
}