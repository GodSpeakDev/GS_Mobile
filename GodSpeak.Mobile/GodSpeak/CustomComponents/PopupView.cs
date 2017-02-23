using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public abstract class PopupView : ContentView
	{		
		private View _content;
		private BoxView _overlay;
		private AbsoluteLayout _layout;

		public PopupView()
		{
			_layout = new AbsoluteLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			_overlay = new BoxView()
			{
				BackgroundColor = Color.Black,
				Color = Color.Black,
				Opacity = 0
			};

			_layout.Children.Add(_overlay, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

			Content = _layout;
		}

		protected abstract View CreateContent();

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (width != -1 && height != -1 && _content == null)
			{
				_content = CreateContent();
				_layout.Children.Add(_content, new Rectangle(0, this.Height, this.Width, AbsoluteLayout.AutoSize), AbsoluteLayoutFlags.None);
			}
		}

		public void Show()
		{
			this.Animate("Showing", new Animation((x) =>
			{
				_content.TranslationY = -x * _content.Height;
				_overlay.Opacity = x * 0.55;
			}));
		}

		public void Hide()
		{
			this.Animate("Hiding", new Animation((x) =>
			{
				_content.TranslationY = -_content.Height + x * _content.Height;
				_overlay.Opacity = 0.55 - (0.55 * x);
			}), finished: (rate, finished) =>
			{
				var parentAbsolute = this.Parent as AbsoluteLayout;
				parentAbsolute.Children.Remove(this);
			});
		}
	}
}
