using MvvmCross.Core.ViewModels;

namespace GodSpeak.ViewModels
{
    public class FirstViewModel : CustomViewModel
    {
        private string _hello = "Hello MvvmCross";
        public string Hello
        { 
            get { return _hello; }
            set { SetProperty (ref _hello, value); }
        }
    }
}
