using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class InputPopup : PopupView
	{
		private Label _titleLabel;
		private Label _messageLabel;
		private CustomEntry _input;
		private Grid _grid;
		private TaskCompletionSource<InputResult> _result;

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
			get { return _buttons; }
			set { _buttons = value; }
		}

		private InputOptions _inputOptions;
		public InputOptions InputOptions
		{
			get { return _inputOptions;}
			set { 
				_inputOptions = value;
				if (_input != null)
				{
					_input.Placeholder = _inputOptions.Placeholder;
				}
			}
		}

		protected override View CreateContent()
		{
			_grid = new Grid()
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
				_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
			}

			_titleLabel = new Label()
			{
				Style = (Style)Application.Current.Resources["AlertSheetTitle"],
				HorizontalTextAlignment = TextAlignment.Center,
				Text = _title,
			};

			_messageLabel = new Label()
			{
				Margin = new Thickness(20, 10, 20, 25),
				Style = (Style)Application.Current.Resources["AlertSheetBody"],
				HorizontalTextAlignment = TextAlignment.Center,
				Text = _message,
			};

			_input = new CustomEntry() 
			{
				Margin = new Thickness(10, 5, 10, 5),
				Style = (Style)Application.Current.Resources["PopupEntry"],
				Placeholder = _inputOptions?.Placeholder,
				HeightRequest = (double) Application.Current.Resources["ElementHeight"]
			};
			_input.Completed += (sender, e) => 
			{
				Finish();
			};

			for (int i = 0; i < Buttons.Length; i++)
			{
				var button = new CustomButton()
				{					
					Style = (Style)Application.Current.Resources[i == 0 ? "BorderButtonWhite" : "BorderButtonTransparent"],
					Margin = new Thickness(10, 5, 10, 5),
					Text = Buttons[i],
				};
				button.Clicked += (sender, e) =>
				{
					Hide();
					var senderText = (sender as Button).Text;
					_result.TrySetResult(new InputResult() {SelectedButton=senderText, InputText=_input.Text});
				};
				_grid.Children.Add(button, 0, 3 + i);
			}

			_grid.Children.Add(_titleLabel, 0, 0);
			_grid.Children.Add(_messageLabel, 0, 1);
			_grid.Children.Add(_input, 0, 2);

			return _grid;
		}

		private void Finish()
		{
			this.Animate("Hiding", new Animation((x) =>
			{
				AbsoluteLayout.SetLayoutBounds(PopupContent, new Rectangle(0, 1 + x, 1, AbsoluteLayout.AutoSize));
				Overlay.Opacity = 0.55 - (0.55 * x);
			}), finished: (rate, finished) =>
			{
				var parentAbsolute = this.Parent as AbsoluteLayout;
				parentAbsolute.Children.Remove(this);
				var senderText = (_grid.Children[0] as Button).Text;
				_result.TrySetResult(new InputResult() { SelectedButton = senderText, InputText = _input.Text });
			});
		}

		public new Task<InputResult> Show()
		{
			this.Animate("Showing", new Animation((x) =>
			{
				AbsoluteLayout.SetLayoutBounds(PopupContent, new Rectangle(0, 2-x, 1, AbsoluteLayout.AutoSize));
				Overlay.Opacity = x * 0.55;
			}), finished: (arg1, arg2) => 
			{								
				_input.Focus();
			});

			_result = new TaskCompletionSource<InputResult>();
			return _result.Task;
		}
	}
}
