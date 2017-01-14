using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class RegisterViewModel : CustomViewModel
	{
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
		public MvxCommand LoginCommand
		{
			get
			{
				return _saveCommand ?? (_saveCommand = new MvxCommand(DoSaveCommand));
			}
		}

		public RegisterViewModel(IDialogService dialogService) : base(dialogService)
		{
			
		}

		private void DoSaveCommand()
		{
			
		}
	}
}
