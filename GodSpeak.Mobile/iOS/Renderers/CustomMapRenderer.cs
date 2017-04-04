using System;
using Xamarin.Forms.Maps.iOS;
using GodSpeak;
using Xamarin.Forms;
using GodSpeak.iOS;
using MapKit;
using Foundation;
using UIKit;
using System.IO;
using System.Drawing;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		private MKMapView NativeMap { get { return (this.NativeView as MapRenderer).Control as MKMapView; } }

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
			{
			base.OnElementChanged(e);

			try
			{
				this.NativeMap.Delegate = null;
				this.NativeMap.Delegate = new MyMapDelegate();
			}
			catch (Exception ex)
			{

			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);


		}
	}

	class MyMapDelegate : MKMapViewDelegate
	{
		string pId = "PinAnnotation";

		public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			if (!(annotation is MKPointAnnotation))
				return null;

			// create pin annotation view
			var anView = (MKAnnotationView)mapView.DequeueReusableAnnotation(pId);

			if (anView == null)
				anView = new MKAnnotationView(annotation, pId);

			anView.Image = GetImage();
			anView.CanShowCallout = true;

			return anView;
		}

		public UIImage GetImage()
		{
			try
			{
				var image = UIImage.FromBundle("oval.png");

				var size = new CoreGraphics.CGSize(10, 10);

				UIGraphics.BeginImageContext(size);
				image.Draw(new CoreGraphics.CGRect(0, 0, size.Width, size.Height));

				var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();

				return resizedImage;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
