using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Services;

namespace GodSpeak
{
	public class ImpactViewModel : CustomViewModel
	{		
		private List<ImpactDay> _allImpactDays;

		private bool _isPlaying;
		public bool IsPlaying
		{
			get { return _isPlaying;}
			set { SetProperty(ref _isPlaying, value);}
		}

		private ObservableCollection<ImpactDay> _shownImpactDays; 
		public ObservableCollection<ImpactDay> ShownImpactDays
		{
			get { return _shownImpactDays; }
			set { SetProperty(ref _shownImpactDays, value);}
		}

		private DateTime _minDate;
		public DateTime MinDate
		{
			get { return _minDate;}
			set { 
				SetProperty(ref _minDate, value);
				RaisePropertyChanged(() => CurrentSliderDate);
			}
		}

		public DateTime CurrentSliderDate
		{
			get 
			{
				return MinDate.AddDays(DayValue);
			}
		}

		private int _dayValue;
		public int DayValue
		{
			get { return _dayValue;}
			set { 
				SetProperty(ref _dayValue, value);
				SetImpactDays();
				RaisePropertyChanged(() => CurrentSliderDate);
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

		private MvxCommand _playCommand;
		public MvxCommand PlayCommand
		{
			get
			{
				return _playCommand ?? (_playCommand = new MvxCommand(DoPlayCommand));
			}
		}

		private MvxCommand _stopCommand;
		public MvxCommand StopCommand
		{
			get
			{
				return _stopCommand ?? (_stopCommand = new MvxCommand(DoStopCommand));
			}
		}

		public ImpactViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{			
			ShownImpactDays = new ObservableCollection<ImpactDay>();

			MinimumDayValue = 0;
			MaximumDayValue = 1;
			DayValue = 1;
		}

		public async void Init()
		{
			this.HudService.Show();

			var currentUser = await SessionService.GetUser();
			var response = await WebApiService.GetImpact(new GetImpactRequest() 
			{
				InviteCode = currentUser.InviteCode
			});
			this.HudService.Hide();

			if (response.IsSuccess)
			{
				_allImpactDays = response.Payload;

				if (_allImpactDays.Count > 0)
				{
					var firstDate = _allImpactDays.OrderBy(x => x.Date).First();
					var lastDate = _allImpactDays.OrderBy(x => x.Date).Last();

					if (firstDate.Date == lastDate.Date)
					{
						MaximumDayValue = 1;
						MinDate = firstDate.Date.Date.AddDays(-1);	
					}
					else
					{
						MaximumDayValue = (lastDate.Date.Date - firstDate.Date).Days;
						MinDate = firstDate.Date.Date;	
					}


					DayValue = MaximumDayValue;
				}
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private void SetImpactDays()
		{
			if (_allImpactDays == null)
				return;

			var firstDate = MinDate;

			var cutDate = firstDate != null ? firstDate.AddDays(DayValue) : DateTime.MinValue;
			cutDate = cutDate.AddDays(1).AddSeconds(-1);

			var daysToBeRemoved = ShownImpactDays.Where(x => x.Date > cutDate).ToList();

			foreach (var impactDay in daysToBeRemoved)
			{
				ShownImpactDays.Remove(impactDay);
			}

			var daysToBeAdded = _allImpactDays.Where(x => x.Date.Date <= cutDate.Date && !ShownImpactDays.Contains(x)).ToList();
			foreach (var impactDay in daysToBeAdded)
			{
				ShownImpactDays.Add(impactDay);
			}

			RaisePropertyChanged(nameof(ShownImpactDays));
		}

		private void DoStopCommand()
		{
			IsPlaying = false;
		}

		private async void DoPlayCommand()
		{
			IsPlaying = true;

			if (DayValue == MaximumDayValue)
			{
				ShownImpactDays.Clear();
				DayValue = MinimumDayValue;
			}

			for (int i = DayValue; i < MaximumDayValue; i++)
			{
				await Task.Delay(200);

				if (!IsPlaying)
				{
					break;
				}

				i += 1;
				DayValue = i;
			}

			IsPlaying = false;
		}
	}
}
