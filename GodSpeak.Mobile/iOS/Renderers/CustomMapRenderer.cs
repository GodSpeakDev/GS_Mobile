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
using Xamarin.Forms.Platform.iOS;
using Google.Maps;
using CoreGraphics;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.iOS
{
	public class CustomMapRenderer : ViewRenderer
	{
		MapView mapView;

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			var camera = CameraPosition.FromCamera(latitude: 37.79,
								longitude: -122.40,
								zoom: 6);
			mapView = MapView.FromCamera (CGRect.Empty, camera);
			mapView.MyLocationEnabled = true;
			this.SetNativeControl(mapView);

            AddCluster ();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
		}

		void AddCluster()
		{
			//var googleMapView = mapView; //use the real mapview init'd somewhere else instead of this
			//var iconGenerator = new GMUDefaultClusterIconGenerator();
			//var algorithm = new GMUNonHierarchicalDistanceBasedAlgorithm();
			//var renderer = new GMUDefaultClusterRenderer(googleMapView, iconGenerator);

			//renderer.WeakDelegate = this;

			//clusterManager = new GMUClusterManager(googleMapView, algorithm, renderer);

			//for (var i = 0; i <= kClusterItemCount; i++)
			//{
			//	var lat = kCameraLatitude + extent * GetRandomNumber(-1.0, 1.0);

			//	var lng = kCameraLongitude + extent * GetRandomNumber(-1.0, 1.0);

			//	var name = $"Item {i}";

			//	IGMUClusterItem item = new POIItem(lat, lng, name);

			//	clusterManager.AddItem(item);
			//}

			//clusterManager.Cluster();

			//clusterManager.SetDelegate(this, this);
		}
	}

	//class MyMapDelegate : MKMapViewDelegate
	//{
	//	string pId = "PinAnnotation";

	//	public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
	//	{
	//		if (!(annotation is MKPointAnnotation))
	//			return null;

	//		// create pin annotation view
	//		var anView = (MKAnnotationView)mapView.DequeueReusableAnnotation(pId);

	//		if (anView == null)
	//			anView = new MKAnnotationView(annotation, pId);

	//		anView.Image = GetImage();
	//		anView.CanShowCallout = true;

	//		return anView;
	//	}

	//	public UIImage GetImage()
	//	{
	//		try
	//		{
	//			var image = UIImage.FromBundle("oval.png");

	//			var size = new CoreGraphics.CGSize(10, 10);

	//			UIGraphics.BeginImageContext(size);
	//			image.Draw(new CoreGraphics.CGRect(0, 0, size.Width, size.Height));

	//			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
	//			UIGraphics.EndImageContext();

	//			return resizedImage;
	//		}
	//		catch (Exception ex)
	//		{
	//			return null;
	//		}
	//	}
	//}
}
