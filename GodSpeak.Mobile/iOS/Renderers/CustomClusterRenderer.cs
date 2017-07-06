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

namespace GodSpeak.iOS
{
	public class CustomClusterRenderer : GMUDefaultClusterRenderer
	{
		public CustomClusterRenderer(MapView mapview, IGMUClusterIconGenerator clusterGenerator) : base(mapview, clusterGenerator)
		{
			
		}

		public override bool ShouldRenderAsCluster(IGMUCluster cluster, float zoom)
		{
			return cluster.Count > 1;
		}
	}
}
