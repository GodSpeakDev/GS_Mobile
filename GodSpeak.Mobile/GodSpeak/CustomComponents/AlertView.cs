using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class AlertView : PopupView
	{
		private Label _titleLabel;
		private Label _messageLabel;
		private Button _okButton;

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

		private string _buttonText;
		public string ButtonText
		{
			get { return _buttonText; }
			set
			{
				_buttonText = value;
				if (_okButton != null)
				{
					_okButton.Text = value;
				}
			}
		}

		protected override View CreateContent()
		{
			var grid = new Grid()
			{
				BackgroundColor = Color.Transparent,
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

			_okButton = new Button()
			{
				FontSize = 18,
				BackgroundColor = ColorHelper.Secondary,
				TextColor = ColorHelper.Primary,
				Margin = new Thickness(10, 30, 10, 5),
				Text = _buttonText,
				FontAttributes = FontAttributes.Bold,
			};
			_okButton.Clicked += (sender, e) => 
			{ 
				Hide(); 
			};

			grid.Children.Add(_titleLabel, 0, 0);
			grid.Children.Add(_messageLabel, 0, 1);
			grid.Children.Add(_okButton, 0, 2);

			return grid;
		}
	}
}
