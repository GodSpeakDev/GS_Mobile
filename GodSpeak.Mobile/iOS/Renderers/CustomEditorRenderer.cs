using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace GodSpeak.iOS
{
	public class CustomEditorRenderer : EditorRenderer
	{
		public CustomEditor CustomEntry
		{
			get { return Element as CustomEditor; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			SetBorderFrame();
		}

		private void SetBorderFrame()
		{
			if (this.Control != null)
			{				
				this.Control.Layer.MasksToBounds = true;
				this.Control.Layer.CornerRadius = 5.0f;
			}
		}
	}
}
