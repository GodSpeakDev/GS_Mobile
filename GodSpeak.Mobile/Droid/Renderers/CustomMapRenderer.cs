using System;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms;
using GodSpeak;
using GodSpeak.Droid;
using Android.Gms.Maps;
using Xamarin.Forms.Maps;
using Android.Gms.Maps.Model;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		private IList<Pin> _pins;
		private Dictionary<Guid, Marker> _markers = new Dictionary<Guid, Marker>();
		private GoogleMap _googleMap;
		private bool isDrawn;

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null) 
			{
                var formsMap = (CustomMap)e.NewElement;
				_pins = formsMap.Pins;
                ((MapView)Control).GetMapAsync(this);
				formsMap.OnAddPin = OnAddPin;
				formsMap.OnRemovePin = OnRemovePin;
            }
		}

		private void OnRemovePin(Guid id)
		{
			var marker = _markers[id];
			marker.Remove();
			_markers.Remove(id);
		}

		private void OnAddPin(Guid id, Pin pin)
		{
			var newMarker = new MarkerOptions();
			newMarker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
			newMarker.SetTitle(pin.Label);
			newMarker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.oval));

			var marker = _googleMap.AddMarker(newMarker);
			_markers.Add(id, marker);

			if (_markers.Count == 1)
			{
				_googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom (marker.Position, 7));
			}
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			var settings = googleMap.UiSettings;
			settings.MyLocationButtonEnabled = false;
			settings.ZoomControlsEnabled = false;

			_googleMap = googleMap;

			_googleMap.Clear();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
		}
	}
}
