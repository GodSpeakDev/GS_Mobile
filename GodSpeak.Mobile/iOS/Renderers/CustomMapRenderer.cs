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
using GodSpeak.Resources;
using System.Linq;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.iOS
{
	public class CustomMapRenderer : ViewRenderer, IGMUClusterRendererDelegate, IGMUClusterManagerDelegate, IMapViewDelegate
	{
		private MapView _mapView;
		private GMUClusterManager _clusterManager;
		private Dictionary<MapPoint, IGMUClusterItem> _markers = new Dictionary<MapPoint, IGMUClusterItem>();
		private CustomIconGenerator _iconGenerator;

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
			_mapView = MapView.FromCamera (CGRect.Empty, camera);

			this.SetNativeControl(_mapView);

			if (e.NewElement != null)
			{
				var formsMap = (CustomMap)e.NewElement;
				formsMap.OnAddPin = OnAddPin;
				formsMap.OnRemovePin = OnRemovePin;
				formsMap.OnClearMap = OnClearMap;
			}

            AddCluster ();
            AddMyOrigin();
		}

		private void OnClearMap()
		{
			_markers.Clear();
			_clusterManager.ClearItems();
			_clusterManager.Cluster();

			AddMyOrigin();
		}

		private void OnRemovePin(MapPoint id)
		{
			if (_markers.ContainsKey(id))
			{
				var marker = _markers[id];

				_clusterManager.RemoveItem(marker);
				_markers.Remove(id);
                _clusterManager.Cluster();

			}
		}

		private void OnAddPin(MapPoint id, Pin pin)
		{
			if (_mapView == null || _markers.ContainsKey(id))
				return;

			var item = new POIItem(pin.Position.Latitude, pin.Position.Longitude, pin.Label);
			_markers.Add(id, item);
			_clusterManager.AddItem(item);
			_clusterManager.Cluster();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
            _clusterManager.Cluster ();
		}

		void AddCluster()
		{
			var googleMapView = _mapView; //use the real mapview init'd somewhere else instead of this
			_iconGenerator = new CustomIconGenerator(
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
					GetImage(25, 25),
                    GetImage(25, 25),
                    GetImage(25, 25),
                    GetImage(25, 25),
                    GetImage(25, 25),
				});

			var algorithm = new CustomAlgorithm();
			var renderer = new CustomClusterRenderer(_mapView, _iconGenerator);
			renderer.WeakDelegate = this;

			_clusterManager = new GMUClusterManager(googleMapView, algorithm, renderer);
			_clusterManager.Cluster();

			_clusterManager.SetDelegate(this, this);
		}

		private void AddMyOrigin()
		{
			var item = new POIItem(SettingsService.Latitude, SettingsService.Longitude, Text.Me);
			_clusterManager.AddItem(item);
			_clusterManager.Cluster();
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

		public UIImage Resize(UIImage image, int height, int width)
		{
			var size = new CoreGraphics.CGSize(width, height);

			UIGraphics.BeginImageContext(size);
			image.Draw(new CoreGraphics.CGRect(0, 0, size.Width, size.Height));

			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return resizedImage;
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
				else if (myMarker.UserData is GMUStaticCluster)
				{
					var cluster = (GMUStaticCluster) myMarker.UserData;
					var size = (int) (15 + Math.Pow(_mapView.Camera.Zoom, 2));
					myMarker.Icon = _iconGenerator.IconForText(new NSString(cluster.Count.ToString()), GetImage(size, size));
				}
			}
		}

		[Export("clusterManager:didTapCluster:")]
		public void DidTapCluster(GMUClusterManager clusterManager, IGMUCluster cluster)
		{
			var newCamera = CameraPosition.FromCamera(cluster.Position, _mapView.Camera.Zoom + 1);

			var update = CameraUpdate.SetCamera(newCamera);

			_mapView.MoveCamera(update);
		}

		public class CustomAlgorithm : GMCluster.GMUGridBasedClusterAlgorithm
		{			
			public override IGMUCluster[] ClustersAtZoom(float zoom)
			{
				// Uncomment to test the zooming hack
				return base.ClustersAtZoom(zoom * 2);
			} 
		}

		public class CustomIconGenerator : GMUDefaultClusterIconGenerator
		{
			private NSNumber[] _buckets;
			private UIImage[] _backgroundImages;
			private NSCache _iconCache;

			public CustomIconGenerator(NSNumber[] buckets, UIImage[] backgroundImages) : base(buckets, backgroundImages)
			{
				_buckets = buckets;
				_backgroundImages = backgroundImages;
				_iconCache = new NSCache();
			}

			public override UIImage IconForSize(nuint size)
			{
				var bucketIndex = BucketIndexForSize((int)size);
				NSString text;

				if (size < _buckets[bucketIndex].UnsignedLongValue) 
				{
					text = new NSString(size.ToString());
  				} 
				else 
				{
					text = new NSString(_buckets[bucketIndex].ToString() + "+");
  				}

			    var image = _backgroundImages[bucketIndex];
				return IconForText(text, image);
			}

			private int BucketIndexForSize(int size)
			{
				var index = 0;
				while (index + 1 < _buckets.Count() && (int) _buckets[index + 1] <= size) 
				{
    				++index;
  				}

  				return index;
			}

			public UIImage IconForText(NSString text, UIImage image)
			{
				var icon = (UIImage) _iconCache.ObjectForKey (text);
  				if (icon != null) 
				{
    				return icon;
  				}

				var font = UIFont.BoldSystemFontOfSize(12);
				CGSize size = image.Size;
				UIGraphics.BeginImageContextWithOptions(size, false, 0.0f);
				image.Draw(new CGRect(0, 0, size.Width, size.Height));
				var rect = new CGRect(0, 0, image.Size.Width, image.Size.Height);

				var paragraphStyle = new NSMutableParagraphStyle();
				paragraphStyle.Alignment = UITextAlignment.Center;

				var attributes = new UIStringAttributes();
				attributes.Font = font;
				attributes.ParagraphStyle = paragraphStyle;
				attributes.ForegroundColor = UIColor.White;

				var textSize = text.GetSizeUsingAttributes(attributes);
				var textRect = rect.Inset((rect.Size.Width - textSize.Width) / 2, (rect.Size.Height - textSize.Height) / 2);
				
				text.DrawString(textRect, attributes);

				var newImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();

				_iconCache.SetObjectforKey(newImage, text);
  				
				return newImage;
			}
		}
	}
}
