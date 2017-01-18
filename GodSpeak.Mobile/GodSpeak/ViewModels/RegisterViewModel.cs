﻿using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using MvvmCross.Core;
using MvvmCross.Forms;
using MvvmCross.Platform;

namespace GodSpeak
{
	public class RegisterViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private ISessionService _sessionService;
		private IMediaPicker _mediaPicker;

		private object _image;
		public object Image
		{
			get { return _image;}
			set { SetProperty(ref _image, value);}
		}

		private string _firstName;
		public string FirstName
		{
			get { return _firstName; }
			set { SetProperty(ref _firstName, value); }
		}

		private string _lastName;
		public string LastName
		{
			get { return _lastName; }
			set { SetProperty(ref _lastName, value); }
		}

		private string _city;
		public string City
		{
			get { return _city; }
			set { SetProperty(ref _city, value); }
		}

		private string _state;
		public string State
		{
			get { return _state; }
			set { SetProperty(ref _state, value); }
		}

		private string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		private MvxCommand _saveCommand;
		public MvxCommand SaveCommand
		{
			get
			{
				return _saveCommand ?? (_saveCommand = new MvxCommand(DoSaveCommand));
			}
		}

		private MvxCommand _choosePictureCommand;
		public MvxCommand ChoosePictureCommand
		{
			get
			{
				return _choosePictureCommand ?? (_choosePictureCommand = new MvxCommand(DoChoosePictureCommand));
			}
		}

		public RegisterViewModel(IDialogService dialogService, IWebApiService webApi, ISessionService sessionService, IMediaPicker mediaPicker) : base(dialogService)
		{
			_webApi = webApi;
			_sessionService = sessionService;
			_mediaPicker = mediaPicker;
		}

		public void Init()
		{
			Image = "http://www.gravatar.com/avatar/a1c6e240931b44f7f4b21492232cd3fc.png?s=160";
		}

		private async void DoSaveCommand()
		{
			if (string.IsNullOrEmpty(FirstName))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.FirstNameRequiredMessage);
				return;
			}

			if (string.IsNullOrEmpty(LastName))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.LastNameRequiredMessage);
				return;
			}

			if (string.IsNullOrEmpty(City))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.CityRequiredMessage);
				return;
			}

			if (string.IsNullOrEmpty(State))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.StateRequiredMessage);
				return;
			}

			if (string.IsNullOrEmpty(Email))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.EmailRequiredMessage);
				return;
			}

			if (string.IsNullOrEmpty(Password))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.PasswordRequiredMessage);
				return;
			}

			var request = new RegisterUserRequest()
			{
				FirstName = FirstName,
				LastName = LastName,
				City = City,
				State = State,
				Email = Email,
				Password = Password,
				ProfilePhoto = Image as byte[]
			};

			var response = await _webApi.RegisterUser(request);

			if (response.IsSuccess)
			{
				await _sessionService.SaveUser(response.Content.Payload);
				this.ShowViewModel<HomeViewModel>();
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private async void DoChoosePictureCommand()
		{
			var menuResponse = await this.DialogService.ShowMenu(Text.PictureSourceQuestion, "Cancel", null, Text.PictureSourceFromGallery, Text.PictureSourceFromCamera);

			MediaFile response;

			if (menuResponse == "Cancel")
			{
				return;
			}
			else if (menuResponse == Text.PictureSourceFromCamera)
			{
				response = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());
			}
			else if (menuResponse == Text.PictureSourceFromGallery)
			{
				response = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions());
			}
			else
			{
				return;
			}

			Image = response.Source.ToByteArray();
		}
	}
}
