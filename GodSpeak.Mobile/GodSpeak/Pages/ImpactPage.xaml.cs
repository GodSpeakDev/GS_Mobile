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
		private Dictionary<Guid, Pin> _pins;

		public ImpactPage()
		{
			_pins = new Dictionary<Guid, Pin>();
			NavigationPage.SetHasNavigationBar(this, false);
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
				foreach (var item in e.OldItems)
				{
					RemoveImpactDayPins((ImpactDay)item);
				}
			}
		}

		private void RemoveImpactDayPins(ImpactDay day)
		{
			foreach (var point in day.MapPoints)
			{
				var pin = _pins[point.MapPointId];
				_pins.Remove(point.MapPointId);
				MyMap.Pins.Remove(pin);
			}
		}

		private void AddImpactDayPins(ImpactDay day)
		{
			var mapPoints = new List<Pin>();
			foreach (var point in day.MapPoints)
			{
				var pin = new Pin()
				{
					Position = new Position(point.Latitude, point.Longitude),
					Type = PinType.Generic,
					Label = point.Title,
				};

				if (!_pins.ContainsKey(point.MapPointId))
				{
					_pins.Add(point.MapPointId, pin);
					mapPoints.Add(pin);
				}
			}

			foreach (var point in mapPoints)
			{				
				MyMap.Pins.Add(point);

				if (MyMap.Pins.Count == 1)
				{
					MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(point.Position, Distance.FromKilometers(50)));
				}
			}
		}
	}
}
