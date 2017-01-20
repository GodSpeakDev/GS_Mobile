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

		private ObservableCollection<CheckModel<MessageCategory>> _categories;
		public ObservableCollection<CheckModel<MessageCategory>> Categories
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
				Categories = new ObservableCollection<CheckModel<MessageCategory>>
					(
						response.Content.Payload.Select(x => new CheckModel<MessageCategory>() 
						{
							IsChecked = x.Enabled,
							Model = x
						}));
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
				Payload = Categories.Where(x => x.IsChecked).Select(x => x.Model).ToList()
			};

			var response = await _webApi.SaveCategories(request);

			if (response.IsSuccess)
			{
				Categories = new ObservableCollection<CheckModel<MessageCategory>>
					(
						response.Content.Payload.Select(x => new CheckModel<MessageCategory>()
						{
							IsChecked = false,
							Model = x
						}));
				await DialogService.ShowAlert(Text.SuccessPopupTitle, Text.SavedCategoriesSuccessful);
			}
			else
			{
				await HandleResponse(response);
			}
		}
	}
}
