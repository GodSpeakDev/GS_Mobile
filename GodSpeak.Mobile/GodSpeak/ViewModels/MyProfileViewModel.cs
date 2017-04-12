using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using MvvmCross.Core;
using MvvmCross.Forms;
using MvvmCross.Platform;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using GodSpeak.Services;
using System.Threading.Tasks;

namespace GodSpeak
{
    public class MyProfileViewModel : CustomViewModel
    {
        private IMediaPicker _mediaPicker;
		private IImageService _imageService;

        private int _selectedCountryIndex;
        public int SelectedCountryIndex {
            get { return _selectedCountryIndex; }
            set {
                SetProperty (ref _selectedCountryIndex, value);
                RaisePropertyChanged (nameof (HasSelectedCountry));
            }
        }

        public bool HasSelectedCountry {
            get { return SelectedCountryIndex >= 0; }
        }

        private string [] _countries;
        public string [] Countries {
            get { return _countries; }
            set { SetProperty (ref _countries, value); }
        }

        protected string [] CountryCodes;

        private object _image;
        public object Image {
            get { return _image; }
            set { SetProperty (ref _image, value); }
        }

        private string _firstName;
        public string FirstName {
            get { return _firstName; }
            set { SetProperty (ref _firstName, value); }
        }

        private string _lastName;
        public string LastName {
            get { return _lastName; }
            set { SetProperty (ref _lastName, value); }
        }

        private string _zipCode;
        public string ZipCode {
            get { return _zipCode; }
            set { SetProperty (ref _zipCode, value); }
        }

        private string _password;
        public string Password {
            get { return _password; }
            set {
                SetProperty (ref _password, value);
                RaisePropertyChanged (nameof (IsPasswordValid));
                RaisePropertyChanged (nameof (IsConfirmPasswordValid));
            }
        }

        public bool IsPasswordValid {
            get 
			{
                return Password.IsValidPassword();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword {
            get { return _confirmPassword; }
            set {
                SetProperty (ref _confirmPassword, value);
                RaisePropertyChanged (nameof (IsConfirmPasswordValid));
            }
        }

        public bool IsConfirmPasswordValid {
            get {
                return string.IsNullOrEmpty (ConfirmPassword) || ConfirmPassword == Password;
            }
        }

        private string _currentPassword;
        public string CurrentPassword {
            get { return _currentPassword; }
            set { SetProperty (ref _currentPassword, value); }
        }

        private MvxCommand _saveCommand;
        public MvxCommand SaveCommand {
            get {
                return _saveCommand ?? (_saveCommand = new MvxCommand (DoSaveCommand));
            }
        }

        private MvxCommand _choosePictureCommand;
        public MvxCommand ChoosePictureCommand {
            get {
                return _choosePictureCommand ?? (_choosePictureCommand = new MvxCommand (DoChoosePictureCommand));
            }
        }

        public MyProfileViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IMediaPicker mediaPicker, IImageService imageService) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            _mediaPicker = mediaPicker;
			_imageService = imageService;
        }

        public async void Init ()
        {
            HudService.Show ();
            var countries = (await WebApiService.GetCountries ()).Payload;
            Countries = countries.Select (c => c.Title).ToArray ();
            CountryCodes = countries.Select (c => c.Code).ToArray ();

			var user = await WebApiService.GetProfile ();
            if (user.IsSuccess) {
                SetUserInfo (user.Payload);
                HudService.Hide ();
                RaiseAllPropertiesChanged ();
            } else {
                await HandleResponse (user);
            }
        }

        private void SetUserInfo (User user)
        {
            SetPhoto (user);
            this.FirstName = user.FirstName;
            this._lastName = user.LastName;
            this._selectedCountryIndex = new List<string> (CountryCodes).IndexOf (user.CountryCode);
            this._zipCode = user.PostalCode;
        }

        private void SetPhoto (User user)
        {
            this._image = user.PhotoUrl;
        }

        private async void DoSaveCommand ()
        {
            HudService.Show ();
            var user = await SessionService.GetUser ();
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.CountryCode = CountryCodes [SelectedCountryIndex];
            user.PostalCode = ZipCode;
            user.CurrentPassword = CurrentPassword;
            user.NewPassword = Password;
            user.PasswordConfirm = ConfirmPassword;

            var response = await WebApiService.SaveProfile (user);
            HudService.Hide ();

            if (response.IsSuccess) {
                this.Close (this);
            } else {
                await HandleResponse (response);
            }
        }

        private async void DoChoosePictureCommand ()
        {
            var menuResponse = await this.DialogService.ShowMenu (Text.PictureSourceQuestion, null, Text.PictureSourceFromGallery, Text.PictureSourceFromCamera, Text.Cancel);

			try
			{
				MediaFile response;

				if (menuResponse == Text.Cancel)
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

				if (response != null)
				{
					this.HudService.Show();

					_imageService.Compress(response, 200, 200);

					var photoResponse = await WebApiService.UploadPhoto(new UploadPhotoRequest()
					{						
						FilePath = response.Path
					});

					System.Diagnostics.Debug.WriteLine("Image Size: {0}", response.Source.ToByteArray().Length);

					this.HudService.Hide();

					if (photoResponse.IsSuccess)
					{
						SetPhoto(photoResponse.Payload);
						RaisePropertyChanged(() => Image);
					}
					else
					{
						await HandleResponse(photoResponse);
					}
				}
			}
			catch (TaskCanceledException ex)
			{

			}
        }
    }
}
