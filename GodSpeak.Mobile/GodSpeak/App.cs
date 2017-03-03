using MvvmCross.Platform.IoC;

namespace GodSpeak
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

			CreatableTypes().Containing("DialogService").AsInterfaces().RegisterAsDynamic();

            RegisterAppStart<GetStartedViewModel>();
        }
    }
}
