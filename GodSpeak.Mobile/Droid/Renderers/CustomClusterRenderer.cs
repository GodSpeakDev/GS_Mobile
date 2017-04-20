using System;
using Com.Google.Maps.Android.Clustering;
using Android.Gms.Maps.Model;
using Com.Google.Maps.Android.UI;
using Android.Widget;
using Xamarin.Forms;
using GodSpeak;
using Xamarin.Forms.Platform.Android;

namespace GodSpeak.Droid
{
	public class CustomClusterRenderer : Com.Google.Maps.Android.Clustering.View.DefaultClusterRenderer
	{
		private IconGenerator _clusterIconGenerator;

		public CustomClusterRenderer(Android.Content.Context context, Android.Gms.Maps.GoogleMap map, ClusterManager clusterManager) : base(context, map, clusterManager)
		{
			_clusterIconGenerator = new IconGenerator(context);
		}

		protected override void OnBeforeClusterItemRendered(Java.Lang.Object obj, MarkerOptions markerOptions)
		{
			markerOptions.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.oval));
		}

		protected override void OnBeforeClusterRendered(ICluster obj, MarkerOptions markerOptions)
		{
			base.OnBeforeClusterRendered(obj, markerOptions);
		}

		protected override int GetColor(int clusterSize)
		{
			return ColorHelper.Primary.ToAndroid();
		}
	}
}
