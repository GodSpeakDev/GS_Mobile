using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class RegisterEntry : CustomEntry
	{
		private bool _isFocused;
		public override bool IsFocused
		{
			get { return _isFocused; }
			set
			{
				_isFocused = value;
				SetUI();
			}
		}

		public RegisterEntry() : base()
		{
			PlaceholderColor = ColorHelper.TextInputPlaceHolder;
			SetUI();
		}

		private void SetUI()
		{
			SetOutlineColor();
			SetBackgroundColor();
			SetTextColor();
			SetFont();
		}

		private void SetOutlineColor()
		{
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				OutlineColor = ColorHelper.Warning;
			}
			else if (IsFocused)
			{
				OutlineColor = ColorHelper.Secondary;
			}
			else if (!IsEnabled)
			{
				OutlineColor = ColorHelper.OutlinePlaceHolder;
			}
			else
			{
				OutlineColor = string.IsNullOrEmpty(Text) ? ColorHelper.OutlinePlaceHolder : ColorHelper.Secondary;
			}
		}

		private void SetTextColor()
		{
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				TextColor = ColorHelper.Secondary;
			}
			else if (IsFocused)
			{
				TextColor = ColorHelper.TextInputFocusedText;
			}
			else if (!IsEnabled)
			{
				TextColor = ColorHelper.TextInputDisabledText;
			}
			else if (!string.IsNullOrEmpty(Text))
			{
				TextColor = ColorHelper.Secondary;
			}
			else
			{
				TextColor = ColorHelper.TextInputPlaceHolder;
			}
		}

		private void SetBackgroundColor()
		{
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				BackgroundColor = ColorHelper.Warning;
			}
			else if (IsFocused)
			{
				BackgroundColor = ColorHelper.Secondary;
			}
			else if (!IsEnabled)
			{
				BackgroundColor = ColorHelper.OutlinePlaceHolder;
			}
			else
			{
				BackgroundColor = Color.Transparent;
			}
		}

		private void SetFont()
		{
			FontAttributes = IsFocused ? FontAttributes.Bold : FontAttributes.None;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			SetUI();
		}
	}
}
