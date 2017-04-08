using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
    public partial class GetStartedPage : CustomContentPage
    {
        float BackgroundRatio;

        public GetStartedPage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);

			InviteCodeEntry.Completed += (sender, e) => 
			{
				(this.BindingContext as GetStartedViewModel).SubmitGiftCodeCommand.Execute();
			};
        }

        protected override void OnBindingContextChanged ()
        {
            base.OnBindingContextChanged ();
            var viewModel = this.BindingContext as GetStartedViewModel;
            if (viewModel != null) {
                viewModel.ShowInviteCodeBox = () => TriggerAnimation (GetStartedView, ClaimInviteCodeView);
                viewModel.ShowGiftCodeSuccessBox = () => TriggerAnimation (ClaimInviteCodeView, GiftCodeSuccessView);
            }
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();
            PanoBackground.HeightRequest = (App.ScreenHeight);
            BackgroundRatio = (float)App.ScreenHeight / 368f;
            PanoBackground.WidthRequest = (1000 * BackgroundRatio);
            PanoBackground.TranslationX = -1 * 70 * BackgroundRatio;
        }

        private void TriggerAnimation (View viewToHide, View viewToShow, bool isGoingBack = false)
        {
            var boxBackgroundPosDict = new Dictionary<View, int> ();
            boxBackgroundPosDict [ClaimInviteCodeView] = 190;
            boxBackgroundPosDict [GiftCodeSuccessView] = 170;


            viewToShow.Opacity = 0;
            viewToShow.IsVisible = true;

            var fadeOutAnimation = new Animation ((x) => {
                viewToHide.Opacity = 1 - (x * 1);
            });

            var fadeInAnimation = new Animation ((x) => {
                viewToShow.Opacity = x;
            });

			Animation panoAnimation = null;
			if (isGoingBack)
			{
				panoAnimation = PanoLayout.GoForwardAnimation(-boxBackgroundPosDict[viewToHide] * BackgroundRatio);
			}
			else
			{
				panoAnimation = PanoLayout.GoForwardAnimation(boxBackgroundPosDict[viewToShow] * BackgroundRatio);
			}

            var animation = new Animation ();
            animation.Add (0, 0.8, fadeOutAnimation);
            animation.Add (0.2, 1, fadeInAnimation);
            animation.Add (0, 0.8, panoAnimation);

            animation.Commit (this, "Toggling", length: 600, finished: (arg1, arg2) => { viewToHide.IsVisible = false; });
        }

		protected override bool OnBackButtonPressed()
		{
			if (ClaimInviteCodeView.IsVisible)
			{
				TriggerAnimation(ClaimInviteCodeView, GetStartedView,true);
				return true;
			}
			else if (GiftCodeSuccessView.IsVisible)
			{
                TriggerAnimation(GiftCodeSuccessView, ClaimInviteCodeView,true);
				return true;
			}
			
			return base.OnBackButtonPressed();
		}
    }
}
