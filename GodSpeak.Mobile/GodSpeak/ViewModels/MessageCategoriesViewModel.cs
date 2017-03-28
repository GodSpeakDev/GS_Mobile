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
	public class MessageCategoriesViewModel : CustomViewModel
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

		private ObservableCollection<MessageCategory> _categories;
		public ObservableCollection<MessageCategory> Categories
		{
			get { return _categories;}
			set { SetProperty(ref _categories, value);}
		}

		public MessageCategoriesViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
		}

		public async void Init()
		{
			var response = await _webApi.GetCategories(new GetCategoriesRequest());
			if (response.IsSuccess)
			{
				Categories = new ObservableCollection<MessageCategory>(response.Payload.Payload);
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private async void DoSaveCommand()
		{
			var request = new SaveCategoriesRequest()
			{
				Payload = Categories.Where(x => x.Enabled).ToList()
			};

			var response = await _webApi.SaveCategories(request);

			if (response.IsSuccess)
			{
				Categories = new ObservableCollection<MessageCategory>(response.Payload.Payload);
				await DialogService.ShowAlert(Text.SuccessPopupTitle, Text.SavedCategoriesSuccessful);
			}
			else
			{
				await HandleResponse(response);
			}
		}
	}
}
