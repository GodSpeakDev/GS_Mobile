using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using GodSpeak;

namespace GodSpeak
{
	public class ImpactMap : CustomMap
	{
		public Dictionary<Guid, Pin> _pins;
		public Dictionary<Guid, Pin> PinsDictionary
		{
			get { return _pins;}
		}

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create<ImpactMap, ObservableCollection<ImpactDay>>(
				p => p.ItemsSource, null, BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);

		public ObservableCollection<ImpactDay> ItemsSource
		{
			get { return (ObservableCollection<ImpactDay>)this.GetValue(ItemsSourceProperty); }
			set { 
				this.SetValue(ItemsSourceProperty, value);
				ItemsSource.CollectionChanged += OnShowImpactChange;
				ResetPins();
			}
		}

		private static void ItemsSourceChanged(BindableObject bindable, ObservableCollection<ImpactDay> oldvalue, ObservableCollection<ImpactDay> newValue)
		{
			var map = (ImpactMap)bindable;
			map.ItemsSource = newValue;
		}
		
		public ImpactMap()
		{
			_pins = new Dictionary<Guid, Pin>();
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

		private void ResetPins()
		{
			foreach (var pin in _pins.Values)
			{
				Pins.Remove(pin);
			}
			_pins = new Dictionary<Guid, Pin>();

			foreach (var day in ItemsSource)
			{
				AddImpactDayPins(day);
			}
		}

		private void RemoveImpactDayPins(ImpactDay day)
		{
			foreach (var point in day.MapPoints)
			{
				var pin = _pins[point.MapPointId];
				_pins.Remove(point.MapPointId);
				Pins.Remove(pin);
				if (OnRemovePin != null)
				{
					OnRemovePin(point.MapPointId);
				}
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
					if (OnAddPin != null)
					{
                        OnAddPin(point.MapPointId, pin);
					}
				}
			}

			foreach (var point in mapPoints)
			{
				Pins.Add(point);

				if (Pins.Count == 1)
				{
					MoveToRegion(MapSpan.FromCenterAndRadius(point.Position, Distance.FromKilometers(50)));
				}
			}
		}
	}
}
