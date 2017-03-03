using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
	public partial class GetStartedPage : CustomContentPage
	{
		public GetStartedPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			var viewModel = this.BindingContext as GetStartedViewModel;
			if (viewModel != null)
			{
				viewModel.ToggleViewVisibility = ToggleViewVisibility;
			}
		}

		private void ToggleViewVisibility()
		{			
			ClaimInviteCodeView.Opacity = 0;
			ClaimInviteCodeView.IsVisible = true;

			var fadeOutAnimation = new Animation((x) =>
			{
				GetStartedView.Opacity = 1 - (x * 1);
			});

			var fadeInAnimation = new Animation((x) =>
			{
				ClaimInviteCodeView.Opacity = x;
			});

			var panoAnimation = PanoLayout.GoForwardAnimation();

			var animation = new Animation();
			animation.Add(0, 0.8, fadeOutAnimation);
			animation.Add(0.2, 1, fadeInAnimation);
			animation.Add(0, 0.8, panoAnimation);

			animation.Commit(this, "Toggling", length: 600, finished: (arg1, arg2) => { GetStartedView.IsVisible = false;});
		}
	}
}
