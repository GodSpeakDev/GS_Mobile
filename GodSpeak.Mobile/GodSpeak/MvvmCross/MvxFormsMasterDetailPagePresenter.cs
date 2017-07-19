using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using System;
using Xamarin.Forms;
using MvvmCross.Forms.Core;

namespace GodSpeak
{
	/// <summary>
	/// Presenter provinding MasterDetailPage functionality for the MainView in a MvxForms App.
	/// </summary>

	// Based on code used MvxFormsPagePresenter code
	public abstract class MvxFormsMasterDetailPagePresenter : MvxViewPresenter
	{
		private Application _mvxFormsApp;

		public Application MvxFormsApp
		{
			get { return _mvxFormsApp; }
			set	
			{
				if (value == null)
				{
					throw new ArgumentException("MvxFormsApp cannot be null");
				}

				_mvxFormsApp = value;
			}
		}

		protected MvxFormsMasterDetailPagePresenter()
		{
		}

		protected MvxFormsMasterDetailPagePresenter(Application mvxFormsApp)
		{
			MvxFormsApp = mvxFormsApp;
		}

		public override void ChangePresentation(MvxPresentationHint hint)
		{
			if (HandlePresentationChange(hint)) return;

			if (hint is MvxClosePresentationHint)
			{
				var mainPage = MvxFormsApp.MainPage as MasterDetailPage;

				if (mainPage == null)
				{
					var navPage = MvxFormsApp.MainPage as NavigationPage;
					if (navPage != null)
					{
						navPage.PopAsync();
						if (navPage.Navigation.NavigationStack.Count == 1)
							RootContentPageActivated();
					}
					else
					{
						Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
					}
				}
				else
				{
					// Perform pop on the Detail Page and launch RootContentPageActivated if root has been reached
					var navPage = mainPage.Detail as NavigationPage;
					navPage.PopAsync();
					if (navPage.Navigation.NavigationStack.Count == 1)
						RootContentPageActivated();
				}
			}
			else if (hint is OpenMenuPresentationHint)
			{
				var mainPage = MvxFormsApp.MainPage as MasterDetailPage;

				if (mainPage == null)
				{
					Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
				}
				else
				{
					mainPage.IsPresented = true;
				}
			}
			else if (hint is CloseMenuPresentationHint)
			{
				var mainPage = MvxFormsApp.MainPage as MasterDetailPage;

				if (mainPage == null)
				{
					Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
				}
				else
				{
					mainPage.IsPresented = false;
				}
			}
		}

		public override void Show(MvxViewModelRequest request)
		{
			if (TryShowPage(request))
				return;

			Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
		}

		protected virtual void CustomPlatformInitialization(Page mainPage)
		{
		}

		private void SetupForBinding(Page page, IMvxViewModel viewModel, MvxViewModelRequest request)
		{
			var mvxContentPage = page as IMvxContentPage;
			if (mvxContentPage != null)
			{
				mvxContentPage.Request = request;
				mvxContentPage.ViewModel = viewModel;
			}
			else {
				page.BindingContext = viewModel;
			}
		}

		private bool TryShowPage(MvxViewModelRequest request)
		{
			var page = MvxPresenterHelpers.CreatePage(request);
			if (page == null)
				return false;

			if (request.PresentationValues != null && request.PresentationValues.ContainsKey("NavigationMode") && request.PresentationValues["NavigationMode"] == "RestoreNavigation")
			{
				_mvxFormsApp.MainPage = new CustomNavigationPage(page);
				var navPage = MvxFormsApp.MainPage as NavigationPage;
				CustomPlatformInitialization(navPage);
			}

			var viewModel = MvxPresenterHelpers.LoadViewModel(request);

			SetupForBinding(page, viewModel, request);

			if (_mvxFormsApp.MainPage == null)
			{
				if (viewModel is MvxMasterDetailViewModel)
				{
					SetMasterDetail(viewModel, page, request);
				}
				else
				{
					_mvxFormsApp.MainPage = new CustomNavigationPage(page);
					var navPage = MvxFormsApp.MainPage as NavigationPage;
					CustomPlatformInitialization(navPage);
				}
			}
			else if (_mvxFormsApp.MainPage is MasterDetailPage)
			{
				var mainPage = _mvxFormsApp.MainPage as MasterDetailPage;

				// Functionality for clearing the navigation stack before pushing to new Page (for example in a menu with multiple options)
				if (request.PresentationValues != null)
				{
					if (request.PresentationValues.ContainsKey("NavigationMode") && request.PresentationValues["NavigationMode"] == "ClearStack")
					{
						mainPage.Detail.Navigation.PopToRootAsync();
						if (Device.Idiom == TargetIdiom.Phone)
							mainPage.IsPresented = false;
					}
				}

				try
				{
					var nav = mainPage.Detail as NavigationPage;

					// calling this sync blocks UI and never navigates hence code continues regardless here
					nav.PushAsync(page);
				}
				catch (Exception e)
				{
					Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
					return false;
				}
			}
			else
			{
				if (viewModel is MvxMasterDetailViewModel)
				{
					SetMasterDetail(viewModel, page, request);
				}
				else
				{
					try
					{
						var navPage = MvxFormsApp.MainPage as NavigationPage;

						// check for modal presentation parameter
						string modalParameter;
						if (request.PresentationValues != null && request.PresentationValues.TryGetValue("modal", out modalParameter) && bool.Parse(modalParameter))
							navPage.Navigation.PushModalAsync(page);
						else
							// calling this sync blocks UI and never navigates hence code continues regardless here
							navPage.PushAsync(page);
					}
					catch (Exception e)
					{
						Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
						return false;
					}
				}
			}

			return true;
		}

		private void SetMasterDetail(IMvxViewModel viewModel, Page page, MvxViewModelRequest request)
		{
			var masterDetailViewModel = viewModel as MvxMasterDetailViewModel;

			Page rootContentPage = null;
			if (masterDetailViewModel.RootContentPageViewModelType != null)
			{                
				var rootContentRequest = new MvxViewModelRequest(masterDetailViewModel.RootContentPageViewModelType, new MvxBundle(request.ParameterValues), null);

				var rootContentViewModel = MvxPresenterHelpers.LoadViewModel(rootContentRequest);
				rootContentPage = MvxPresenterHelpers.CreatePage(rootContentRequest);
				SetupForBinding(rootContentPage, rootContentViewModel, rootContentRequest);
			}
			else
			{
				rootContentPage = new ContentPage();
			}

			var navPage = new CustomNavigationPage(rootContentPage);

			//Hook to Popped event to launch RootContentPageActivated if proceeds
			navPage.Popped += (sender, e) =>
			{
				if (navPage.Navigation.NavigationStack.Count == 1)
					RootContentPageActivated();
			};

			var mainPage = new MasterDetailPage
			{
				Master = page,
				Detail = navPage
			};

			_mvxFormsApp.MainPage = mainPage;
			CustomPlatformInitialization(mainPage);
		}

		private void RootContentPageActivated()
		{
			var mainPage = Application.Current.MainPage as MasterDetailPage;
			if (mainPage != null)
			{
				(mainPage.Master.BindingContext as MvxMasterDetailViewModel)?.RootContentPageActivated();
			}
		}
	}
}