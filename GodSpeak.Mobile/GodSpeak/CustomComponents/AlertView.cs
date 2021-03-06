﻿using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class AlertView : PopupView
	{
		private Label _titleLabel;
		private Label _messageLabel;
		private Button _okButton;
		private TaskCompletionSource<bool> _result;

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
				Style = (Style)Application.Current.Resources["AlertSheetTitle"],
				HorizontalTextAlignment=TextAlignment.Center,
				Text = _title,
			};

			_messageLabel = new Label()
			{
				Margin = new Thickness(20, 10, 20, 0),
				Style = (Style)Application.Current.Resources["AlertSheetBody"],
				HorizontalTextAlignment = TextAlignment.Center,
				Text = _message,
			};

			_okButton = new CustomButton()
			{				
				Style = (Style)Application.Current.Resources["ButtonWhite"],
				Margin = new Thickness(10, 30, 10, 5),
				Text = _buttonText,
			};

			_okButton.Clicked += (sender, e) => 
			{ 				
				Hide(); 
				_result.TrySetResult(true);
			};

			grid.Children.Add(_titleLabel, 0, 0);
			grid.Children.Add(_messageLabel, 0, 1);
			grid.Children.Add(_okButton, 0, 2);

			return grid;
		}

		public new Task Show()
		{
			base.Show();
			_result = new TaskCompletionSource<bool>();
			return _result.Task;
		}
	}
}
