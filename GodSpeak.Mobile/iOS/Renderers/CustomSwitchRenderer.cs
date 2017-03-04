using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace GodSpeak.iOS
{
	public class CustomSwitchRenderer : SwitchRenderer
	{
		public CustomSwitchRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged(e);
			SetOutlineColor();
		}

		private void SetOutlineColor()
		{
			if (this.Control != null)
			{
				this.Control.TintColor = ColorHelper.LightGray.ToUIColor();
			}
		}

		private void SetCircleColor()
		{
			if (this.Control != null)
			{
				this.Control.ValueChanged += (sender, e) => 
				{
					this.Control.ThumbTintColor = this.Control.On ? UIColor.White : ColorHelper.DarkGray.ToUIColor();
				}; 
			}
		}
	}
}
