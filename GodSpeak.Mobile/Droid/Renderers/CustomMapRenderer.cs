using System;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms;
using GodSpeak;
using GodSpeak.Droid;
using Android.Gms.Maps;
using Xamarin.Forms.Maps;
using Android.Gms.Maps.Model;
using System.Collections.Generic;
using MvvmCross.Platform;
using System.Threading.Tasks;
using Com.Google.Maps.Android.Clustering;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
	{		
		private Dictionary<MapPoint, ClusterItem> _markers = new Dictionary<MapPoint, ClusterItem>();
		private GoogleMap _googleMap;
		ClusterManager _clusterManager;

		private ISettingsService SettingsService
		{
			get
			{
				return MvvmCross.Platform.Mvx.Resolve<ISettingsService>();
			}
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null) 
			{
                var formsMap = (CustomMap)e.NewElement;
                ((MapView)Control).GetMapAsync(this);
				formsMap.OnAddPin = OnAddPin;
				formsMap.OnRemovePin = OnRemovePin;
            }
		}

		private void OnRemovePin(MapPoint id)
		{
			if (_markers.ContainsKey(id))
			{
				var marker = _markers[id];

				_clusterManager.RemoveItem(marker);
				_markers.Remove(id);
			}
		}

		private void OnAddPin(MapPoint id, Pin pin)
		{
			if (_googleMap == null || _markers.ContainsKey(id))
				return;

			var clusterItem = new ClusterItem(pin.Position.Latitude, pin.Position.Longitude);
			_markers.Add(id, clusterItem);
			_clusterManager.AddItem(clusterItem);
			_clusterManager.Cluster();
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			var settings = googleMap.UiSettings;
			settings.MyLocationButtonEnabled = false;
			settings.ZoomControlsEnabled = false;

			_googleMap = googleMap;
			_googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(SettingsService.Latitude, SettingsService.Longitude), 1));

			_googleMap.Clear();

			_clusterManager = new ClusterManager(this.Context, _googleMap);
			_clusterManager.SetOnClusterClickListener(this);
			_clusterManager.SetOnClusterItemClickListener(this);
			_clusterManager.SetRenderer(new CustomClusterRenderer(this.Context, _googleMap, _clusterManager));
			                            
			_googleMap.SetOnCameraChangeListener(_clusterManager);
			_googleMap.SetOnMarkerClickListener(_clusterManager);

			var customMap = this.Element as ImpactMap;
			foreach (var item in customMap.PinsDictionary)
			{
				OnAddPin(item.Key, item.Value);
			}
		}

		public bool OnClusterClick(ICluster cluster)
		{			
			return false;
		}

		public bool OnClusterItemClick(Java.Lang.Object marker)
		{			
			return false;
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
		}
	}

	public class ClusterItem : Java.Lang.Object, IClusterItem
	{
		public ClusterItem()
		{
		}
		public LatLng Position { get; set; }

		public ClusterItem(double lat, double lng)
		{
			Position = new LatLng(lat, lng);
		}		
	}
}
