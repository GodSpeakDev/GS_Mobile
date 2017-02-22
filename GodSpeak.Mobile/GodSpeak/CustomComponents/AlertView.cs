using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class AlertView : ContentView
	{
		private Label _titleLabel;
		private Label _messageLabel;
		private View _content;
		private BoxView _overlay;
		private AbsoluteLayout _layout;

		private string _title;
		public string Title
		{
			get { return _title;}
			set 
			{
				_title = value;
				if (_titleLabel != null)
				{
					_titleLabel.Text = value;
				}
			}
		}

		private string _message;
		public string Message
		{
			get { return _message;}
			set 
			{
				_message = value;
				if (_messageLabel != null)
				{
					_messageLabel.Text = value;
				}
			}
		}

		public AlertView() 
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

		private View CreateContent()
		{
			var grid = new Grid()
			{
				BackgroundColor = ColorHelper.Primary,
				Padding = new Thickness(5, 20, 5, 5),
				RowDefinitions = new RowDefinitionCollection()
				{
					new RowDefinition() {Height=new GridLength(1, GridUnitType.Auto)},
					new RowDefinition() {Height=new GridLength(1, GridUnitType.Auto)},
					new RowDefinition() {Height=new GridLength(1, GridUnitType.Auto)},
				}
			};

			_titleLabel = new Label()
			{
				TextColor = ColorHelper.Secondary,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				Text = _title,
				FontSize = 24
			};

			_messageLabel = new Label()
			{
				Margin = new Thickness(20, 10, 20, 0),
				TextColor = ColorHelper.Secondary,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				Text = _message,
				FontSize = 15
			};

			var okButton = new Button()
			{
				FontSize = 18,
				BackgroundColor = ColorHelper.Secondary,
				TextColor = ColorHelper.Primary,
				Margin = new Thickness(10, 30, 10, 5),
				Text = GodSpeak.Resources.Text.OkPopup,
				FontAttributes = FontAttributes.Bold,
			};
			okButton.Clicked += (sender, e) => 
			{ 
				Hide(); 
			};

			grid.Children.Add(_titleLabel, 0, 0);
			grid.Children.Add(_messageLabel, 0, 1);
			grid.Children.Add(okButton, 0, 2);

			return grid;
		}

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
			}), finished: (rate,finished) => 
			{
				var parentAbsolute = this.Parent as AbsoluteLayout;
				parentAbsolute.Children.Remove(this);
			});	
		}
	}
}
