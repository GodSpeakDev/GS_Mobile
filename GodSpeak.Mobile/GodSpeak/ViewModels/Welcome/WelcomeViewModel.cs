using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class WelcomeViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private int _selectedPosition;
		public int SelectedPosition
		{
			get { return _selectedPosition;}
			set { SetProperty(ref _selectedPosition, value);}
		}

		private ObservableCollection<CustomViewModel> _pages;
		public ObservableCollection<CustomViewModel> Pages
		{
			get { return _pages; }
			set { SetProperty(ref _pages, value); }
		}

		public WelcomeViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;	
		}

		public void Init()
		{
			var pages = new List<CustomViewModel>();
			pages.Add(new GetStartedViewModel(this, DialogService));
			pages.Add(new ClaimInviteCodeViewModel(this, DialogService, _webApi));
			pages.Add(new RequestInviteCodeViewModel(this, DialogService, _webApi));

			Pages = new ObservableCollection<CustomViewModel>(pages);
		}

		public void SelectPage<T>() where T : CustomViewModel
		{
			SelectedPosition = Pages.IndexOf(Pages.FirstOrDefault (x => x is T));
		}
	}
}
