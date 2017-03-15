using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
	public partial class MyProfilePage : CustomContentPage
	{
		public MyProfilePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var profileViewModel = this.BindingContext as MyProfileViewModel;

			if (profileViewModel != null)
			{
				Countries.Items.Clear();
				foreach (var item in profileViewModel.Countries)
				{
					Countries.Items.Add(item);
				}

				Countries.SelectedIndex = profileViewModel.SelectedCountryIndex;
			}
		}
	}
}
