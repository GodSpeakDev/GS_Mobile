using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace GodSpeak
{
	public class Pager : ContentView
	{
		public static readonly BindableProperty NumberOfDotsProperty = BindableProperty.Create("NumberOfDots", typeof(int), typeof(Pager), default(int), propertyChanged: OnNumberOfDotsPropertyChanged);

		public int NumberOfDots
		{
			get { return (int)GetValue(NumberOfDotsProperty); }
			set { 
				SetValue(NumberOfDotsProperty, value);
			}
		}

		public static readonly BindableProperty SelectedDotIndexProperty = BindableProperty.Create("SelectedDotIndex", typeof(int), typeof(Pager), default(int), propertyChanged: OnSelectedDotIndexPropertyChanged);

		public int SelectedDotIndex
		{
			get { return (int)GetValue(SelectedDotIndexProperty); }
			set { SetValue(SelectedDotIndexProperty, value); }
		}

		private StackLayout _dotsContainer;
		private List<Dot> _dots;

		public Pager()
		{
			_dotsContainer = new StackLayout() 
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Orientation = StackOrientation.Horizontal
			};
			_dots = new List<Dot>();

			Margin = new Thickness(0, 10);
			Content = _dotsContainer;
		}

		private void CreateDots()
		{
			_dotsContainer.Children.Clear();
			_dots.Clear();

			for (int i = 0; i < NumberOfDots; i++)
			{
				var dot = new Dot();
				_dots.Add(dot);
				_dotsContainer.Children.Add(dot);
			}

			AnimateDotSelection();
		}

		private void AnimateDotSelection()
		{
			var oldSelectedDot = _dots.FirstOrDefault(x => x.IsSelected);
			var newSelectedDot = _dots[SelectedDotIndex];

			var animation = new Animation();

			if (oldSelectedDot != null)
			{
				var deselectionAnimation = new Animation((obj) => 
				{
					var opacityDiffence = Dot.SelectedColor.A - Dot.UnselectedColor.A;
					oldSelectedDot.BackgroundColor = new Color(Dot.SelectedColor.R, Dot.SelectedColor.G, Dot.SelectedColor.B, (Dot.SelectedColor.A - obj * opacityDiffence));
				}, finished: () => oldSelectedDot.IsSelected = false);

				animation.Add(0, 1, deselectionAnimation);
			}

			var selectionAnimation = new Animation((obj) =>
				{
					var opacityDiffence = Dot.SelectedColor.A - Dot.UnselectedColor.A;
					newSelectedDot.BackgroundColor = new Color(Dot.SelectedColor.R, Dot.SelectedColor.G, Dot.SelectedColor.B, (Dot.UnselectedColor.A + obj * opacityDiffence));
				}, finished: () => newSelectedDot.IsSelected = true);

			animation.Add(0, 1, selectionAnimation);

			AnimationExtensions.Animate(this, "Deselection", animation, length: 50);
		}

		private static void OnNumberOfDotsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var pager = (Pager)bindable;
			pager.CreateDots();
		}

		private static void OnSelectedDotIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var pager = (Pager)bindable;
			pager.AnimateDotSelection();
		}

		public class Dot : ContentView
		{
			public static Color SelectedColor = new Color(255, 255, 255, 1);
			public static Color UnselectedColor = new Color(255, 255, 255, 0.3);

			public bool IsSelected
			{
				get;
				set;
			}

			public Dot()
			{
				BackgroundColor = UnselectedColor;
				HeightRequest = 8;
				WidthRequest = 8;
			}
		}
	}
}
