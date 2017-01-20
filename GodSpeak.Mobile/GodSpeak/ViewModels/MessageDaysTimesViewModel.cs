using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class MessageDaysTimesViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private MvxCommand _saveCommand;
		public MvxCommand SaveCommand
		{
			get
			{
				return _saveCommand ?? (_saveCommand = new MvxCommand(DoSaveCommand));
			}
		}

		private ObservableCollection<CheckModel<DayOfWeekSettings>> _settings;
		public ObservableCollection<CheckModel<DayOfWeekSettings>> Settings
		{
			get { return _settings; }
			set { SetProperty(ref _settings, value); }
		}

		public MessageDaysTimesViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
		}

		public async void Init()
		{
			var response = await _webApi.GetMessageConfig(new GetMessageConfigRequest());

			if (response.IsSuccess)
			{
				Settings = new ObservableCollection<CheckModel<DayOfWeekSettings>>
					(
						response.Content.Payload.Select(x => new CheckModel<DayOfWeekSettings>() 
						{
							IsChecked = x.Enabled,
							Model = x
						})
					);
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private async void DoSaveCommand()
		{
			var request = new SetMessagesConfigRequest()
			{
				Settings = Settings.Select(x => x.Model).ToList()
			};

			var response = await _webApi.SetMessagesConfigUser(request);

			if (response.IsSuccess)
			{
				Settings = new ObservableCollection<CheckModel<DayOfWeekSettings>>
					(
						response.Content.Payload.Select(x => new CheckModel<DayOfWeekSettings>()
						{
							IsChecked = x.Enabled,
							Model = x
						})
					);
				await DialogService.ShowAlert(Text.SuccessPopupTitle, Text.SavedCategoriesSuccessful);
			}
			else
			{
				await HandleResponse(response);
			}
		}
	}
}
