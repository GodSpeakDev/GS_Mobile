using System;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms;
using GodSpeak;
using GodSpeak.Droid;
using Android.Gms.Maps;
using Xamarin.Forms.Maps;
using Android.Gms.Maps.Model;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GodSpeak.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);
			var mapView = this.Control as MapView;
			if (mapView != null)
			{
				mapView.GetMapAsync(this);
			}
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			var settings = googleMap.UiSettings;
			settings.MyLocationButtonEnabled = false;
			settings.ZoomControlsEnabled = false;

			googleMap.Clear();

			var map = this.Element as Map;

			foreach (var pin in map.Pins)
			{
				var newMarker = new MarkerOptions();
				newMarker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
				newMarker.SetTitle(pin.Label);
				newMarker.SetSnippet(pin.Address);
				newMarker.InvokeIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icon));

				googleMap.AddMarker(newMarker);
			}
		}
	}
}
