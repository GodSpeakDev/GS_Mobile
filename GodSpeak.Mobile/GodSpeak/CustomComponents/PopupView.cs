using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public abstract class PopupView : ContentView
	{		
		protected View PopupContent;
		protected BoxView Overlay;
		protected AbsoluteLayout Layout;

		public PopupView()
		{
			Layout = new AbsoluteLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			Overlay = new BoxView()
			{
				BackgroundColor = Color.Black,
				Color = Color.Black,
				Opacity = 0
			};

			Layout.Children.Add(Overlay, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

			Content = Layout;
		}

		protected abstract View CreateContent();

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (width != -1 && height != -1 && PopupContent == null)
			{
				PopupContent = new GradientBoxView() 
				{
					Content = CreateContent(),
					Colors = new Color[] {ColorHelper.BlueGradientEnd, ColorHelper.Primary}
				};
				Layout.Children.Add(PopupContent, new Rectangle(0, this.Height, this.Width, AbsoluteLayout.AutoSize), AbsoluteLayoutFlags.None);
			}
		}

		public void Show()
		{
			this.Animate("Showing", new Animation((x) =>
			{
				PopupContent.TranslationY = -x * PopupContent.Height;
				Overlay.Opacity = x * 0.55;
			}));
		}

		public void Hide()
		{
			this.Animate("Hiding", new Animation((x) =>
			{
				PopupContent.TranslationY = -PopupContent.Height + x * PopupContent.Height;
				Overlay.Opacity = 0.55 - (0.55 * x);
			}), finished: (rate, finished) =>
			{
				var parentAbsolute = this.Parent as AbsoluteLayout;
				parentAbsolute.Children.Remove(this);
			});
		}
	}
}
