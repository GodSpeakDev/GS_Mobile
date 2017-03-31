using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
    public class DayOfWeekSettings : MvxViewModel
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

        public TimeSpan StartTime {
            get;
            set;
        }

        public TimeSpan EndTime {
            get;
            set;
        }

        public int NumOfMessages {
            get;
            set;
        }
    }
}
