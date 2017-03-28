using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GodSpeak
{
	public partial class ClaimedGiftPage : ContentView
	{
		public ClaimedGiftPage()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var claimedGiftViewModel = this.BindingContext as ClaimedGiftViewModel;

			if (claimedGiftViewModel != null)
			{
				SortOptions.Items.Clear();
				foreach (var item in claimedGiftViewModel.SortOptions)
				{
					SortOptions.Items.Add(item);
				}

				SortOptions.SelectedIndex = 0;
			}
		}
	}
}
