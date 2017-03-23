using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using MvvmCross.Core;
using MvvmCross.Forms;
using MvvmCross.Platform;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GodSpeak
{
	public class MyProfileViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private ISessionService _sessionService;
		private IMediaPicker _mediaPicker;

		private int _selectedCountryIndex;
		public int SelectedCountryIndex
		{
			get { return _selectedCountryIndex; }
			set
			{
				SetProperty(ref _selectedCountryIndex, value);
				RaisePropertyChanged(nameof(HasSelectedCountry));
			}
		}

		public bool HasSelectedCountry
		{
			get { return SelectedCountryIndex > 0; }
		}

		private string[] _countries;
		public string[] Countries
		{
			get { return _countries; }
			set { SetProperty(ref _countries, value); }
		}

		private object _image;
		public object Image
		{
			get { return _image; }
			set { SetProperty(ref _image, value); }
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

		private string _zipCode;
		public string ZipCode
		{
			get { return _zipCode; }
			set { SetProperty(ref _zipCode, value); }
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				SetProperty(ref _password, value);
				RaisePropertyChanged(nameof(IsPasswordValid));
				RaisePropertyChanged(nameof(IsConfirmPasswordValid));
			}
		}

		public bool IsPasswordValid
		{
			get
			{
				var numberDetector = new Regex(@"\d{1}?");
				var lowerCaseDetector = new Regex(@"[a-z]{1}?");
				var upperCaseDetector = new Regex(@"[A-Z]{1}?");

				return string.IsNullOrEmpty(Password) || (numberDetector.IsMatch(Password) && lowerCaseDetector.IsMatch(Password) && upperCaseDetector.IsMatch(Password));
			}
		}

		private string _confirmPassword;
		public string ConfirmPassword
		{
			get { return _confirmPassword; }
			set
			{
				SetProperty(ref _confirmPassword, value);
				RaisePropertyChanged(nameof(IsConfirmPasswordValid));
			}
		}

		public bool IsConfirmPasswordValid
		{
			get
			{
				return string.IsNullOrEmpty(ConfirmPassword) || ConfirmPassword == Password;
			}
		}

		private string _currentPassword;
		public string CurrentPassword
		{
			get { return _currentPassword;}
			set { SetProperty(ref _currentPassword, value);}
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

		public MyProfileViewModel(IDialogService dialogService, IWebApiService webApi, ISessionService sessionService, IMediaPicker mediaPicker) : base(dialogService)
		{
			_webApi = webApi;
			_sessionService = sessionService;
			_mediaPicker = mediaPicker;
		}

		public void Init()
		{
			Countries = new string[]
			{
				"Country",
				"USA"
			};

			var user = _sessionService.GetUser();
			this.Image = user.PhotoUrl;
			this.FirstName = user.FirstName;
			this.LastName = user.LastName;
			this.SelectedCountryIndex = new List<string>(Countries).IndexOf(user.Country);
			this.ZipCode = user.ZipCode;
		}

		private async void DoSaveCommand()
		{
			await DialogService.ShowAlert(Text.MyProfileSuccessTitle, Text.MyProfileSuccessText, Text.AnonymousSuccessButtonTitle);
			this.Close(this);
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
