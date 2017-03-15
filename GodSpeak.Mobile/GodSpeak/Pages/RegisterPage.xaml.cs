using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
	public partial class RegisterPage : CustomContentPage
	{
		public RegisterPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var registerViewModel = this.BindingContext as RegisterViewModel;

			if (registerViewModel != null)
			{
				Countries.Items.Clear();
				foreach (var item in registerViewModel.Countries)
				{
					Countries.Items.Add(item);
				}

				Countries.SelectedIndex = 0;
			}
		}
	}
}
