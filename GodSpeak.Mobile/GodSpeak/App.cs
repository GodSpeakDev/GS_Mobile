using MvvmCross.Platform.IoC;
using MvvmCross;
using MvvmCross.Platform;
using GodSpeak.Api;

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
				.ExcludeInterfaces(typeof(IWebApiService))
                .RegisterAsLazySingleton ();

			Mvx.LazyConstructAndRegisterSingleton<IWebApiService, AzureWebApiService>();

            CreatableTypes ().Containing ("DialogService").AsInterfaces ().RegisterAsDynamic ();

            RegisterAppStart<GetStartedViewModel> ();

        }
    }
}
