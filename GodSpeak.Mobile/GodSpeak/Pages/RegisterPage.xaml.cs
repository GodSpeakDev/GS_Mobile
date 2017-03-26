using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
	public partial class RegisterPage : CustomContentPage
	{
		public RegisterPage()
		{
			PreventKeyboardOverlap = true;
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			FirstNameEntry.Completed += (sender, e) => 
			{
				LastNameEntry.Focus();
			};

			LastNameEntry.Completed += (sender, e) => 
			{
				Countries.Focus();
			};

			var lastSelectedIndex = 0;
			Countries.Focused += (sender, e) => 
			{
				lastSelectedIndex = (sender as CustomPicker).SelectedIndex;	
			};

			Countries.Unfocused += (sender, e) => 
			{
				var actualIndex = (sender as CustomPicker).SelectedIndex;
				if (lastSelectedIndex != actualIndex)
				{
					ZipCodeEntry.Focus();
				}
			};

			ZipCodeEntry.Completed += (sender, e) => 
			{
				EmailEntry.Focus();
			};

			EmailEntry.Completed += (sender, e) =>
			{
				PasswordEntry.Focus();
			};

			PasswordEntry.Completed += (sender, e) =>
			{
				PasswordConfirmEntry.Focus();
			};

			PasswordConfirmEntry.Completed += (sender, e) =>
			{				
				(this.BindingContext as RegisterViewModel).SaveCommand.Execute();			
			};
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
