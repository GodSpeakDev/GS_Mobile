using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using MvvmCross.Core;
using MvvmCross.Forms;
using MvvmCross.Platform;
using System.Threading.Tasks;
using System.Linq;
using GodSpeak.Services;

namespace GodSpeak
{
    public class RegisterViewModel : CustomViewModel
    {
        private IMediaPicker _mediaPicker;
        private IImageService _imageService;
        private MediaFile _response;

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

        private string _email;
        public string Email {
            get { return _email; }
            set {
                SetProperty (ref _email, value);
                RaisePropertyChanged (nameof (IsEmailValid));
            }
        }

        public bool IsEmailValid {
            get {
                return string.IsNullOrEmpty (Email) || Email.Contains ("@");
            }
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
            get {
                return Password.IsValidPassword ();
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

        private MvxCommand _alreadyRegisteredCommand;
        public MvxCommand AlreadyRegisteredCommand {
            get {
                return _alreadyRegisteredCommand ?? (_alreadyRegisteredCommand = new MvxCommand (DoAlreadyRegisteredCommand));
            }
        }

        public RegisterViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IMediaPicker mediaPicker, IImageService imageService) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            _mediaPicker = mediaPicker;
            _imageService = imageService;
        }

        string _inviteCode;

        public async void Init (string inviteCode)
        {
            _inviteCode = inviteCode;
            //Image = "http://www.gravatar.com/avatar/a1c6e240931b44f7f4b21492232cd3fc.png?s=160";
            Image = "profile_placeholder.png";
            await PopulateCountries ();
        }

        async Task PopulateCountries ()
        {
            var countries = (await WebApiService.GetCountries ()).Payload;
            Countries = countries.Select (c => c.Title).ToArray ();
            CountryCodes = countries.Select (c => c.Code).ToArray ();
        }

        private async void DoSaveCommand ()
        {					
			if (!(await ValidateForm ()))
                return;

            var request = new RegisterUserRequest () {
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = Email,
                Password = Password,
                PasswordConfirm = ConfirmPassword,
                CountryCode = CountryCodes [SelectedCountryIndex],
                PostalCode = ZipCode,
                InviteCode = _inviteCode
            };

            HudService.Show ();
            var response = await WebApiService.RegisterUser (request);

            if (response.IsSuccess) 
			{
                if (Image != null && Image != "profile_placeholder.png") {
                    var photoResponse = await WebApiService.UploadPhoto (new UploadPhotoRequest () 
					{                        
                        FilePath = _response.Path
                    });
                    HudService.Hide ();

                    if (photoResponse.IsSuccess) 
					{
                    	await SuccessfullRegister(response.Payload);    
                    } 
					else 
					{
                        await HandleResponse (photoResponse);
                    }
                } 
				else 
				{
                    HudService.Hide ();
					await SuccessfullRegister(response.Payload);
                }
            } 
			else 
			{
                HudService.Hide ();
                await HandleResponse (response);
            }
        }

		private async Task SuccessfullRegister(User user)
		{
			await SessionService.SaveUser(user);
			var result = await DialogService.ShowMenu(Text.Impact, Text.ImpactQuestion, Text.Yes, Text.No);
			if (result == Text.Yes)
			{
				this.ShowViewModel<WhoReferredYouViewModel>();
			}
			else
			{				
				this.ShowViewModel<HomeViewModel>(new {comesFromRegisterFlow=true});		
			}
		}

        private async void DoChoosePictureCommand ()
        {
            var menuResponse = await this.DialogService.ShowMenu (Text.PictureSourceQuestion, null, Text.PictureSourceFromGallery, Text.PictureSourceFromCamera, Text.Cancel);

            try {

                if (menuResponse == Text.Cancel) {
                    return;
                } else if (menuResponse == Text.PictureSourceFromCamera) {
                    _response = await _mediaPicker.TakePhotoAsync (new CameraMediaStorageOptions ());
                } else if (menuResponse == Text.PictureSourceFromGallery) {
                    _response = await _mediaPicker.SelectPhotoAsync (new CameraMediaStorageOptions ());
                } else {
                    return;
                }

                if (_response != null) {
                    HudService.Show ();
                    _imageService.Compress (_response, 200, 200);
                    Image = _response.Path;
                    HudService.Hide ();
                }
            } catch (TaskCanceledException ex) {

            }
        }

        private async void DoAlreadyRegisteredCommand ()
        {
            this.ShowViewModel<LoginViewModel> ();
        }

        private async Task<bool> ValidateForm ()
        {
    //        if (string.IsNullOrEmpty(FirstName))
    //        {
    //          	await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.FirstNameRequiredMessage);
				//return false;
    //        }

    //        if (string.IsNullOrEmpty(LastName))
    //        {
    //        	await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.LastNameRequiredMessage);
				//return false;
    //        }

    //        if (string.IsNullOrEmpty(Email))
    //        {
    //          await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.EmailRequiredMessage);
				//return false;
    //        }

    //        if (string.IsNullOrEmpty(Password))
    //        {
    //          	await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.PasswordRequiredMessage);
				//return false;
    //        }

            return true;
        }
    }
}
