using System;
using Com.Google.Maps.Android.Clustering;
using Android.Gms.Maps.Model;
using Com.Google.Maps.Android.UI;
using Android.Widget;
using Xamarin.Forms;
using GodSpeak;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using System.Collections.Generic;

namespace GodSpeak.Droid
{
	public class CustomClusterRenderer : Com.Google.Maps.Android.Clustering.View.DefaultClusterRenderer
	{
		private IconGenerator _clusterIconGenerator;
        private float _density;
        private ShapeDrawable _coloredCircleBackground;
        private Dictionary<int, BitmapDescriptor> _iconCache = new Dictionary<int, BitmapDescriptor>();

		public CustomClusterRenderer(Android.Content.Context context, Android.Gms.Maps.GoogleMap map, ClusterManager clusterManager) : base(context, map, clusterManager)
		{
            _density = context.Resources.DisplayMetrics.Density;
			_clusterIconGenerator = new CustomIconGenerator(context);
            _clusterIconGenerator.SetContentView(MakeSquareTextView(context));
            _clusterIconGenerator.SetTextAppearance(Resource.Style.ClusterIcon_TextAppearance);
            _clusterIconGenerator.SetBackground(MakeClusterBackground());
            _clusterIconGenerator.SetContentPadding((int)(9 * _density), (int)(4 * _density), (int)(9 * _density), (int)(4 * _density));
		}

		protected override void OnBeforeClusterItemRendered(Java.Lang.Object obj, MarkerOptions markerOptions)
		{
			markerOptions.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.oval));
		}

		protected override void OnBeforeClusterRendered(ICluster obj, MarkerOptions markerOptions)
		{
            base.OnBeforeClusterRendered(obj, markerOptions);

            var bucket = GetBucket(obj);

            BitmapDescriptor icon;
            if (_iconCache.ContainsKey(bucket))
            {
                icon = _iconCache[bucket];
            }
            else
            {
                _coloredCircleBackground.Paint.Color = new Android.Graphics.Color(GetColor(bucket));
                icon = BitmapDescriptorFactory.FromBitmap(_clusterIconGenerator.MakeIcon(GetClusterText(bucket)));
                _iconCache.Add(bucket,icon);
            }

            markerOptions.SetIcon(icon);
		}

		protected override int GetColor(int clusterSize)
		{
			return ColorHelper.Primary.ToAndroid();
		}

		private SquareTextView MakeSquareTextView(Android.Content.Context context)
		{
			SquareTextView squareTextView = new SquareTextView(context);
			ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			squareTextView.LayoutParameters = layoutParams;
            squareTextView.SetTextAppearance(Resource.Style.ClusterIcon_TextAppearance);
            squareTextView.Id = Resource.Id.amu_text;
            squareTextView.TextSize = 11;
			int twelveDpi = (int)(12 * _density);
			squareTextView.SetPadding(twelveDpi, twelveDpi, twelveDpi, twelveDpi);
			return squareTextView;
		}

		private LayerDrawable MakeClusterBackground()
		{            
			_coloredCircleBackground = new ShapeDrawable(new OvalShape());
			ShapeDrawable outline = new ShapeDrawable(new OvalShape());
            outline.Paint.Color = Android.Graphics.Color.ParseColor("#80ffffff"); // Transparent white.
			LayerDrawable background = new LayerDrawable(new Drawable[] { outline, _coloredCircleBackground });
            int strokeWidth = (int)(_density * 3);
			background.SetLayerInset(1, strokeWidth, strokeWidth, strokeWidth, strokeWidth);
			return background;
		}
	}

    public class CustomIconGenerator : IconGenerator
    {
        private TextView _textView;
        public CustomIconGenerator(Android.Content.Context context) : base(context)
        {  
            
        }

        public override void SetContentView(Android.Views.View p0)
        {
            base.SetContentView(p0);
            _textView = p0.FindViewById<TextView>(Resource.Id.amu_text);
        }

        public override Bitmap MakeIcon(Java.Lang.ICharSequence p0)
        {
            if (_textView != null)
            {
                _textView.Text = p0.ToString();    
            }

            return base.MakeIcon(p0);
        }
    }
}
