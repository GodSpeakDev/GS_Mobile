using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
    public partial class LoginPage : CustomContentPage
    {
        public LoginPage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);

			EmailEntry.Completed += (sender, e) => 
			{
				PasswordEntry.Focus();
			};

			PasswordEntry.Completed += (sender, e) => 
			{
				(this.BindingContext as LoginViewModel).LoginCommand.Execute();
			};
        }
    }
}
