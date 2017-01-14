using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class WelcomeViewModel : CustomViewModel
	{
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

		public WelcomeViewModel()
		{
			
		}

		public void Init()
		{
			var pages = new List<CustomViewModel>();
			pages.Add(new GetStartedViewModel(this));
			pages.Add(new ClaimInviteCodeViewModel(this));
			pages.Add(new RequestInviteCodeViewModel());

			Pages = new ObservableCollection<CustomViewModel>(pages);
		}

		public void SelectPage<T>() where T : CustomViewModel
		{
			SelectedPosition = Pages.IndexOf(Pages.FirstOrDefault (x => x is T));
		}
	}
}
