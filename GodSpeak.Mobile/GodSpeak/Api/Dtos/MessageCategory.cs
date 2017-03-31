using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
    public class MessageCategory : MvxNotifyPropertyChanged
    {
        public Guid Id {
            get;
            set;
        }

        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { SetProperty (ref _enabled, value); }
        }

        public string Title {
            get;
            set;
        }
    }
}
