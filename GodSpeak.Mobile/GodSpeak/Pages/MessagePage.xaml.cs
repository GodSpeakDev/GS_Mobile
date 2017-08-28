using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GodSpeak
{
    public partial class MessagePage : CustomContentPage
    {
        public MessagePage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);
            //MessagesListView.DeselectOnTap ();
        }

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var viewModel = this.BindingContext as MessageViewModel;

			if (viewModel != null)
			{
				viewModel.HighlightHint = HighlightHint;
			}
		}

		private void HighlightHint(MenuItem item)
		{
			if (item.HintMode == MenuItem.HintModes.External)
			{
				HintLabel.Text = item.Hint;

				foreach (var menuItem in MenuItems.Children)
				{
					if (menuItem.BindingContext == item)
					{
						AbsoluteLayout.SetLayoutBounds(HintLabel, new Rectangle(0, menuItem.Y + (MenuItems.Parent as Grid).Y, 1, AbsoluteLayout.AutoSize));
					}
				}
			}
			else
			{
				HintLabel.Text = string.Empty;
			}
		}
    }
}
