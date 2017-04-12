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
		public Dictionary<MapPoint, Pin> _pins;
		public Dictionary<MapPoint, Pin> PinsDictionary
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
			_pins = new Dictionary<MapPoint, Pin>();
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
			_pins = new Dictionary<MapPoint, Pin>();

			foreach (var day in ItemsSource)
			{
				AddImpactDayPins(day);
			}
		}

		private void RemoveImpactDayPins(ImpactDay day)
		{
			foreach (var point in day.Points)
			{
				var pin = _pins[point];
				_pins.Remove(point);
				Pins.Remove(pin);
				if (OnRemovePin != null)
				{
					OnRemovePin(point);
				}
			}
		}

		private void AddImpactDayPins(ImpactDay day)
		{
			var mapPoints = new List<Pin>();
			foreach (var point in day.Points)
			{
				var pin = new Pin()
				{
					Position = new Position(point.Latitude, point.Longitude),
					Type = PinType.Generic,
					Label = day.Date.ToString("d")
				};

				if (!_pins.ContainsKey(point))
				{
					_pins.Add(point, pin);
					mapPoints.Add(pin);
					if (OnAddPin != null)
					{
                        OnAddPin(point, pin);
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
