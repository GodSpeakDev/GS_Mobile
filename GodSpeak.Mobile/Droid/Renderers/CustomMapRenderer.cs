//using System;
//using Xamarin.Forms.Maps.Android;
//using Xamarin.Forms;
//using GodSpeak;
//using GodSpeak.Droid;
//using Android.Gms.Maps;
//using Xamarin.Forms.Maps;
//using Android.Gms.Maps.Model;
//using System.Collections.Generic;
//using MvvmCross.Platform;
//using System.Threading.Tasks;
//using Com.Google.Maps.Android.Clustering;

//[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
//namespace GodSpeak.Droid
//{
//	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
//	{
//		private IList<Pin> _pins;
//		private Dictionary<MapPoint, Marker> _markers = new Dictionary<MapPoint, Marker>();
//		private GoogleMap _googleMap;
//		ClusterManager _clusterManager;

//		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
//		{
//			base.OnElementChanged(e);
//			if (e.NewElement != null) 
//			{
//                var formsMap = (CustomMap)e.NewElement;
//				_pins = formsMap.Pins;
//                ((MapView)Control).GetMapAsync(this);
//				formsMap.OnAddPin = OnAddPin;
//				formsMap.OnRemovePin = OnRemovePin;
//            }
//		}

//		private void OnRemovePin(MapPoint id)
//		{
//			if (_markers.ContainsKey(id))
//			{
//				var marker = _markers[id];
//				marker.Remove();
//				_markers.Remove(id);
//			}
//		}

//		private void OnAddPin(MapPoint id, Pin pin)
//		{
//			if (_googleMap == null || _markers.ContainsKey(id))
//				return;

//			var newMarker = new MarkerOptions();
//			newMarker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
//			newMarker.SetTitle(pin.Label);
//			newMarker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.oval));

//			var marker = _googleMap.AddMarker(newMarker);
//			_markers.Add(id, marker);

//			if (_markers.Count == 1)
//			{
				
//			}
//		}

//		public void OnMapReady(GoogleMap googleMap)
//		{
//			var settings = googleMap.UiSettings;
//			settings.MyLocationButtonEnabled = false;
//			settings.ZoomControlsEnabled = false;

//			_googleMap = googleMap;

//			_googleMap.Clear();

//			_clusterManager = new ClusterManager(this.Context, _googleMap);
//			_clusterManager.SetOnClusterClickListener(this);
//			_clusterManager.SetOnClusterItemClickListener(this);
//			_googleMap.SetOnCameraChangeListener(_clusterManager);
//			_googleMap.SetOnMarkerClickListener(_clusterManager);

//			//Task.Run(async () => 
//			//{
//			//	var user = await Mvx.Resolve<ISessionService>().GetUser();
//			//	if (user != null)
//			//	{
//			//		Device.BeginInvokeOnMainThread(() =>
//			//		{
//			//			_googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(user.Latitude, user.Longitude), 7));
//			//		});
//			//	}	
//			//});

//			//var customMap = this.Element as ImpactMap;
//			//foreach (var item in customMap.PinsDictionary)
//			//{
//			//	OnAddPin(item.Key, item.Value);
//			//}
//			AddClusterItems();
//		}

//		private void AddClusterItems()
//		{
//			var random = new Random ();
//			var items = new List<ClusterItem>();
//			// Create a log. spiral of markers to test clustering
//			for (int i = 0; i< 20; ++i)
//			{				
//				items.Add(new ClusterItem((float) (38.4 + (random.Next(0, 3000) / 10000.0)), (float) (-90.1 + (random.Next(0, 3000) / 10000.0))));
//			}

//			_clusterManager.AddItems(items);
//		}

//		public bool OnClusterClick(ICluster cluster)
//		{
//			//Toast.MakeText(this, cluster.Items.Count + " items in cluster", ToastLength.Short).Show();
//			return false;
//		}

//		public bool OnClusterItemClick(Java.Lang.Object marker)
//		{
//			//Toast.MakeText(this, "Marker clicked", ToastLength.Short).Show();
//			return false;
//		}

//		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//		{
//			base.OnElementPropertyChanged(sender, e);
//		}
//	}

//	public class ClusterItem : Java.Lang.Object, IClusterItem
//	{
//		public ClusterItem()
//		{
//		}
//		public LatLng Position { get; set; }

//		public ClusterItem(double lat, double lng)
//		{
//			Position = new LatLng(lat, lng);
//		}		
//	}
//}
