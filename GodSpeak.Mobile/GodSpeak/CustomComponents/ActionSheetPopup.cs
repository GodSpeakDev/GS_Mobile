using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class ActionSheetPopup : PopupView
	{
		private CustomLabel _titleLabel;
		private CustomLabel _messageLabel;
		private TaskCompletionSource<string> _result;

		private string _title;
		public string Title
		{
			get { return _title; }
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
			get { return _message; }
			set
			{
				_message = value;
				if (_messageLabel != null)
				{
					_messageLabel.Text = value;
				}
			}
		}

		private string[] _buttons;
		public string[] Buttons
		{
			get { return _buttons;}
			set { _buttons = value;}
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
				}
			};

			for (int i = 0; i < Buttons.Length; i++)
			{
				grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)});
			}

			_titleLabel = new CustomLabel()
			{
				Style = (Style)Application.Current.Resources["AlertSheetTitle"],
				HorizontalTextAlignment = TextAlignment.Center,
				Text = _title,
			};

			_messageLabel = new CustomLabel()
			{
				Margin = new Thickness(20, 10, 20, 25),
				Style = (Style)Application.Current.Resources["AlertSheetBody"],
				HorizontalTextAlignment = TextAlignment.Center,
				Text = _message,
			};

			for (int i = 0; i < Buttons.Length; i++)
			{				
				var button = new CustomButton()
				{
					Style = (Style)Application.Current.Resources[i != Buttons.Length-1 ? "BorderButtonWhite" : "BorderButtonTransparent"],
					Margin = new Thickness(10, 5, 10, 5),
					Text = Buttons[i],
				};

				button.Clicked += (sender, e) =>
				{
					Hide();
					var senderText = (sender as Button).Text;
					_result.TrySetResult(senderText);
				};
				grid.Children.Add(button, 0, 2 + i);			
			}

			grid.Children.Add(_titleLabel, 0, 0);
			grid.Children.Add(_messageLabel, 0, 1);


			return grid;
		}

		public new Task<string> Show()
		{
			base.Show();
			_result = new TaskCompletionSource<string>();
			return _result.Task;
		}
	}
}
