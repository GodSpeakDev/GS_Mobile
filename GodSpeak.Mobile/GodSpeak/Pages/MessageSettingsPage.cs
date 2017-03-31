using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
    public partial class MessageSettingsPage : CustomContentPage
    {
        public MessageSettingsPage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);
        }

        protected override void OnDisappearing ()
        {
            base.OnDisappearing ();
            (BindingContext as MessageSettingsViewModel).GoSaveCommand.Execute ();
        }

    }
}
