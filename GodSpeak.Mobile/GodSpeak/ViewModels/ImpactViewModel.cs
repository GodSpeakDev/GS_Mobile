using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class ImpactViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private List<ImpactDay> _allImpactDays;

		private ObservableCollection<ImpactDay> _shownImpactDays; 
		public ObservableCollection<ImpactDay> ShownImpactDays
		{
			get { return _shownImpactDays; }
			set { SetProperty(ref _shownImpactDays, value);}
		}

		private int _dayValue;
		public int DayValue
		{
			get { return _dayValue;}
			set { 
				SetProperty(ref _dayValue, value);
				SetImpactDays();
			}
		}

		private int _minimumDayValue;
		public int MinimumDayValue
		{
			get { return _minimumDayValue;}
			set { SetProperty(ref _minimumDayValue, value);}
		}

		private int _maximumDayValue;
		public int MaximumDayValue
		{
			get { return _maximumDayValue; }
			set { SetProperty(ref _maximumDayValue, value); }
		}

		public ImpactViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
			ShownImpactDays = new ObservableCollection<ImpactDay>();

			MinimumDayValue = 0;
			MaximumDayValue = 1;
		}

		public async void Init()
		{
			var response = await _webApi.GetImpact(new GetImpactRequest());
			if (response.IsSuccess)
			{
				_allImpactDays = response.Content.Payload;

				var firstDate = _allImpactDays.OrderBy(x => x.Date).First();
				var lastDate = _allImpactDays.OrderBy(x => x.Date).Last();

				MaximumDayValue = (lastDate.Date.Date - firstDate.Date).Days + 1;
				DayValue = MaximumDayValue;
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private void SetImpactDays()
		{
			var cutDate = _allImpactDays.OrderBy(x => x.Date).First().Date.Date.AddDays(DayValue - 1);
			var daysToBeRemoved = ShownImpactDays.Where(x => x.Date > cutDate).ToList();

			foreach (var impactDay in daysToBeRemoved)
			{
				ShownImpactDays.Remove(impactDay);
			}

			var daysToBeAdded = _allImpactDays.Where(x => x.Date.Date >= cutDate.Date && !ShownImpactDays.Contains(x)).ToList();
			foreach (var impactDay in daysToBeAdded)
			{
				ShownImpactDays.Add(impactDay);
			}
		}
	}
}
