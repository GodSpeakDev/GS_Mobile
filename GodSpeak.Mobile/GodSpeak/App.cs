using MvvmCross.Platform.IoC;
using MvvmCross;
using MvvmCross.Platform;
using GodSpeak.Api;
using MvvmCross.Plugins.Messenger;
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
    }
}
