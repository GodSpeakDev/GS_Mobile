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
				Layout.Children.Add(PopupContent, new Rectangle(0, 2, this.Width, AbsoluteLayout.AutoSize), AbsoluteLayoutFlags.YProportional|AbsoluteLayoutFlags.WidthProportional);
			}
		}

		public void Show()
		{
			if (PopupContent != null)
			{
				this.Animate("Showing", new Animation((x) =>
				{
					AbsoluteLayout.SetLayoutBounds(PopupContent, new Rectangle(0, 2 - x, 1, AbsoluteLayout.AutoSize));
					Overlay.Opacity = x * 0.55;
				}));
			}
		}

		public void Hide()
		{
			if (PopupContent != null)
			{
				this.Animate("Hiding", new Animation((x) =>
				{
					AbsoluteLayout.SetLayoutBounds(PopupContent, new Rectangle(0, 1 + x, 1, AbsoluteLayout.AutoSize));
					Overlay.Opacity = 0.55 - (0.55 * x);
				}), finished: (rate, finished) =>
				{
					var parentAbsolute = this.Parent as AbsoluteLayout;
					parentAbsolute.Children.Remove(this);
				});
			}
		}
	}
}
