using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GodSpeak
{
	public partial class ImpactPage : CustomContentPage
	{
		public ImpactPage()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var viewModel = (ImpactViewModel) this.BindingContext;
			viewModel.ShownImpactDays.CollectionChanged += OnShowImpactChange;
		}

		private void OnShowImpactChange(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems)
				{
					AddImpactDayPins((ImpactDay)item);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (var item in e.NewItems)
				{
					RemoveImpactDayPins((ImpactDay)item);
				}
			}
		}

		private void RemoveImpactDayPins(ImpactDay day)
		{
			
		}

		private void AddImpactDayPins(ImpactDay day)
		{
			var mapPoints = day.MapPoints.Select(x => new Pin() 
			{
				Position = new Position(x.Latitude, x.Longitude),
				Type = PinType.Generic,
				Label = x.Title,
			});

			foreach (var point in mapPoints)
			{
				MyMap.Pins.Add(point);

				if (MyMap.Pins.Count == 1)
				{
					MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(point.Position, Distance.FromKilometers(10)));
				}
			}
		}
	}
}
