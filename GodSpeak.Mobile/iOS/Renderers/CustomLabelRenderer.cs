using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;
using Foundation;

[assembly: ExportRendererAttribute (typeof (CustomLabel), typeof (CustomLabelRenderer))]
namespace GodSpeak.iOS
{
    public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabel CustomLabel {
            get { return Element as CustomLabel; }
        }

        public CustomLabelRenderer ()
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged (e);
            SetFontWeight ();
        }

        protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);
            SetFontWeight ();
        }

        private void SetFontWeight ()
        {
            if (this.Control == null || this.CustomLabel == null)
                return;


            this.Control.Font = this.CustomLabel.GetUIFont ();


        }



        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
        }
    }
}
