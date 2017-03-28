using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GodSpeak.Resources;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class ClaimedGiftViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private IShareService _shareService;

		public ClaimedGiftViewModel(IDialogService dialogService, IWebApiService webApi, IShareService shareService) : base(dialogService)
		{
			_webApi = webApi;
			_shareService = shareService;

			SortOptions = new string[] 
			{
				Text.SortByClaimDate
			};
		}

		private int _selectedSortIndex;
		public int SelectedSortIndex
		{
			get { return _selectedSortIndex;}
			set { SetProperty(ref _selectedSortIndex, value); }
		}

		private string[] _sortOptions;
		public string[] SortOptions
		{
			get { return _sortOptions; }
			set { SetProperty(ref _sortOptions, value); }
		}

		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible;}
			set { SetProperty(ref _isVisible, value);}
		}

		private ObservableCollection<UserModel> _users;
		public ObservableCollection<UserModel> Users
		{
			get { return _users;}
			set { SetProperty(ref _users, value);}
		}

		public async Task Init()
		{
			Users = new ObservableCollection<UserModel>				
			{
				new UserModel() 
				{
					Name="Dave Ortinau",
					Image="http://www.gravatar.com/avatar/a1c6e240931b44f7f4b21492232cd3fc.png?s=160",
					ClaimedDate=new DateTime(2017,12,30),
					PeopleGifted=3
				},
				new UserModel() 
				{
					Name="Dave Ortinau",
					Image="http://www.gravatar.com/avatar/a1c6e240931b44f7f4b21492232cd3fc.png?s=160",
					ClaimedDate=new DateTime(2017,1,3),
					PeopleGifted=0
				}
			};				
		}

		public class UserModel : MvxViewModel
		{
			private string _image;
			public string Image
			{
				get { return _image;}
				set { SetProperty(ref _image, value);}
			}

			private string _name;
			public string Name
			{
				get { return _name;}
				set { SetProperty(ref _name, value);}
			}

			private DateTime _claimedDate;
			public DateTime ClaimedDate
			{
				get { return _claimedDate; }
				set 
				{ 
					SetProperty(ref _claimedDate, value);
					RaisePropertyChanged(() => ClaimedDateDescription);
				}
			}

			public string ClaimedDateDescription
			{
				get { return string.Format(Text.ClaimedDateDescription, ClaimedDate); }
			}

			private int _peopleGifted;
			public int PeopleGifted
			{
				get { return _peopleGifted; }
				set { SetProperty(ref _peopleGifted, value); }
			}
		}
	}
}
