using MvvmCross.Platform.IoC;

namespace GodSpeak
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {

        public static int ScreenHeight = 640;
        public static int ScreenWidth = 320;

        public override void Initialize ()
        {			
            CreatableTypes ()
                .EndingWith ("Service")
                .AsInterfaces ()
                .RegisterAsLazySingleton ();

            CreatableTypes ().Containing ("DialogService").AsInterfaces ().RegisterAsDynamic ();
            RegisterAppStart<GetStartedViewModel> ();
        }
    }
}
