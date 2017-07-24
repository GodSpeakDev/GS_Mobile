﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;
using System.Diagnostics;

[assembly: ExportRendererAttribute(typeof(GodSpeak.MenuItemView), typeof(MenuItemViewRenderer))]
namespace GodSpeak.iOS
{
	public class MenuItemViewRenderer : VisualElementRenderer<GodSpeak.MenuItemView>
	{
		private void CreateCircle()
		{
			try
			{
				double min = Math.Min(Element.Width, Element.Height);
				this.Layer.CornerRadius = (float)(min / 2.0);
				this.Layer.MasksToBounds = false;
				this.ClipsToBounds = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to create circle image: " + ex);
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			CreateCircle();
		}
	}
}
