using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GodSpeak
{
	public partial class SecondaryButton : Button
	{
		public SecondaryButton()
		{			
			FontSize = 18;
			FontAttributes = FontAttributes.Bold;
			SetUI();
		}

		public void SetUI()
		{
			if (IsEnabled)
			{
				SetEnabledState();
			}
			else
			{
				SetDisabledState();
			}
		}

		public void SetEnabledState()
		{
			TextColor = ColorHelper.Primary;
			BackgroundColor = ColorHelper.Secondary;
		}

		public void SetDisabledState()
		{
			BackgroundColor = ColorHelper.DisabledGray;
			TextColor = ColorHelper.TextInputDisabledText;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == IsEnabledProperty.PropertyName)
			{
				SetUI();
			}
		}
	}
}
