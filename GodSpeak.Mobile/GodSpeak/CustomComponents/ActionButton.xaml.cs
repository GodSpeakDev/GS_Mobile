using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GodSpeak
{
    public partial class ActionButton : Button
    {
        public ActionButton ()
        {
            InitializeComponent ();            
            //SetUI ();
        }

        public void SetUI ()
        {
            if (IsEnabled) {
                SetEnabledState ();
            } else {
                SetDisabledState ();
            }
        }

        public void SetEnabledState ()
        {
            TextColor = ColorHelper.Secondary;
            BackgroundColor = ColorHelper.Primary;
        }

        public void SetDisabledState ()
        {
            BackgroundColor = ColorHelper.DisabledGray;
            TextColor = ColorHelper.TextInputDisabledText;
        }

        protected override void OnPropertyChanged (string propertyName = null)
        {
            base.OnPropertyChanged (propertyName);
            if (propertyName == IsEnabledProperty.PropertyName) {
                //SetUI ();
            }
        }
    }
}
