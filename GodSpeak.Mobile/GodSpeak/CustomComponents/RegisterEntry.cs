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
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				ElementState = ElementState.Error;
			}
			else if (IsFocused)
			{
				ElementState = ElementState.Focused;
			}
			else if (!IsEnabled)
			{
				ElementState = ElementState.Disabled;
			}
			else if (string.IsNullOrEmpty(Text))
			{
				ElementState = ElementState.NotFocusedEmpty;
				OutlineColor = ColorHelper.OutlinePlaceHolder;
			}
			else
			{
				ElementState = ElementState.NotFocusedFilled;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			SetUI();
		}
	}
}
