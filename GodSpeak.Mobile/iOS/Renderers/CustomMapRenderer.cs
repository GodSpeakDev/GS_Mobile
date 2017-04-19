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
using GMCluster;
using Xamarin.Forms.Maps;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.iOS
{
	public class CustomMapRenderer : ViewRenderer, IGMUClusterRendererDelegate, IGMUClusterManagerDelegate, IMapViewDelegate
	{
		MapView mapView;
		GMUClusterManager clusterManager;
		private Dictionary<MapPoint, IGMUClusterItem> _markers = new Dictionary<MapPoint, IGMUClusterItem>();

		private ISettingsService SettingsService
		{
			get 
			{
				return MvvmCross.Platform.Mvx.Resolve<ISettingsService>();
			}
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			var camera = CameraPosition.FromCamera(latitude: SettingsService.Latitude,
								longitude: SettingsService.Longitude,
								zoom: 0);
			mapView = MapView.FromCamera (CGRect.Empty, camera);
			//AddMyOrigin();

			this.SetNativeControl(mapView);

			if (e.NewElement != null)
			{
				var formsMap = (CustomMap)e.NewElement;
				formsMap.OnAddPin = OnAddPin;
				formsMap.OnRemovePin = OnRemovePin;
			}

            AddCluster ();
		}

		private void OnRemovePin(MapPoint id)
		{
			if (_markers.ContainsKey(id))
			{
				var marker = _markers[id];

				clusterManager.RemoveItem(marker);
				_markers.Remove(id);
			}
		}

		private void OnAddPin(MapPoint id, Pin pin)
		{
			if (mapView == null || _markers.ContainsKey(id))
				return;

			var item = new POIItem(pin.Position.Latitude, pin.Position.Longitude, pin.Label);

			_markers.Add(id, item);
			clusterManager.AddItem(item);
			clusterManager.Cluster();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
		}

		void AddCluster()
		{
			var googleMapView = mapView; //use the real mapview init'd somewhere else instead of this
			var iconGenerator = new GMUDefaultClusterIconGenerator(
				new NSNumber[] 
				{ 
					10, 
					50, 
					100, 
					200, 
					1000 
				},
				new UIImage[] 
				{ 
					GetImage(40, 40),
                    GetImage(40, 40),
                    GetImage(40, 40),
                    GetImage(40, 40),
                    GetImage(40, 40),
				});

			var algorithm = new GMUNonHierarchicalDistanceBasedAlgorithm();
			var renderer = new GMUDefaultClusterRenderer(googleMapView, iconGenerator);

			renderer.WeakDelegate = this;

			clusterManager = new GMUClusterManager(googleMapView, algorithm, renderer);
			clusterManager.Cluster();

			clusterManager.SetDelegate(this, this);
		}

		private void AddMyOrigin()
		{
			var marker = new Google.Maps.Marker();
			marker.Position = new CoreLocation.CLLocationCoordinate2D(SettingsService.Latitude, SettingsService.Longitude);
			marker.Title = "Me";
			marker.Map = mapView;
		}

		public UIImage GetImage(int height, int width)
		{
			try
			{
				var image = UIImage.FromBundle("oval.png");

				var size = new CoreGraphics.CGSize(width, height);

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

		[Export("renderer:willRenderMarker:")]
		public void WillRenderMarker(GMUClusterRenderer renderer, Overlay marker)
		{
			if (marker is Marker)
			{ 
				// Overlays sneaking in here disguised as Markers...
				var myMarker = (Marker)marker;

				if (myMarker.UserData is POIItem)
				{
					POIItem item = (POIItem)myMarker.UserData;
					myMarker.Title = item.Name;
					myMarker.Icon = GetImage(15, 15);
				}
				else
				{
					
				}
			}
		}

		[Export("clusterManager:didTapCluster:")]
		public void DidTapCluster(GMUClusterManager clusterManager, IGMUCluster cluster)
		{
			var newCamera = CameraPosition.FromCamera(cluster.Position, mapView.Camera.Zoom + 1);

			var update = CameraUpdate.SetCamera(newCamera);

			mapView.MoveCamera(update);
		}
	}
}
