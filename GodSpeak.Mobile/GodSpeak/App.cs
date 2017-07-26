using MvvmCross.Platform.IoC;
using MvvmCross;
using MvvmCross.Platform;
using GodSpeak.Api;
using MvvmCross.Plugins.Messenger;
using Xamarin.Forms;

namespace GodSpeak
{
    public partial class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public static int ScreenHeight = 640;
        public static int ScreenWidth = 320;

        public override void Initialize ()
        {
            InitializeComponent ();
            CreatableTypes ()
                .EndingWith ("Service")
                .AsInterfaces ()
                .ExcludeInterfaces (typeof (IWebApiService))
                .RegisterAsLazySingleton ();

            Mvx.LazyConstructAndRegisterSingleton<IWebApiService, AzureWebApiService> ();
			Mvx.LazyConstructAndRegisterSingleton<IMvxMessenger, MvxMessengerHub>();

            CreatableTypes ().Containing ("DialogService").AsInterfaces ().RegisterAsDynamic ();

			var settingsService = Mvx.Resolve<ISettingsService>();
			if (!string.IsNullOrEmpty(settingsService.Token))
			{
                RegisterAppStart<HomeViewModel>();
            }
            else
            {
				RegisterAppStart<GetStartedViewModel> ();                
            }
        }

        public static void SetupDependencyInjection()
        {
            
        }

		public static void RefreshCurrentPage()
		{
			var page = Xamarin.Forms.Application.Current.MainPage;

			if (page is MasterDetailPage)
			{
				var masterVM = ((MasterDetailPage)page).Master.BindingContext as CustomViewModel;
				if (masterVM != null)
				{
					masterVM.OnAppearing();
				}

				var detail = ((MasterDetailPage)page).Detail;
				if (detail is NavigationPage)
				{
					var currentVM = ((NavigationPage)detail).CurrentPage.BindingContext as CustomViewModel;
					if (currentVM != null)
					{
						currentVM.OnAppearing();
					}
				}
				else
				{
					var vm = detail.BindingContext as CustomViewModel;
					if (vm != null)
					{
						vm.OnAppearing();
					}
				}
			}
			else if (page is NavigationPage)
			{
				var currentVM = ((NavigationPage)page).CurrentPage.BindingContext as CustomViewModel;
				if (currentVM != null)
				{
					currentVM.OnAppearing();
				}
			}
		}
    }
}
