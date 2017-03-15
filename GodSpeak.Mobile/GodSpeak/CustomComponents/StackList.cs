using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GodSpeak
{
	public class StackList : StackLayout
	{
		public static readonly BindableProperty ItemTappedCommandProperty =
			BindableProperty.Create<StackList, ICommand>(x => x.ItemTappedCommand, null);

		public ICommand ItemTappedCommand
		{
			get { return (ICommand)GetValue(ItemTappedCommandProperty); }
			set { SetValue(ItemTappedCommandProperty, value); }
		}

		protected StackLayout stackLayout;

		public StackList()
		{
			//stackLayout = new StackLayout () {
			//    HorizontalOptions = LayoutOptions.FillAndExpand,
			//    Orientation = StackOrientation.Horizontal,
			//    Padding = 0,
			//    Spacing = 0

			//};
			//Content = stackLayout;
		}

		public event EventHandler SelectedItemChanged;

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create<StackList, IList>(p => p.ItemsSource, default(IList), BindingMode.TwoWay, null, ItemsSourceChanged);

		public IList ItemsSource
		{
			get
			{
				return (IList)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<StackList, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay, null, OnSelectedItemChanged);

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create<StackList, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		private static void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
		{
			var itemsLayout = (StackList)bindable;
			itemsLayout.SetItems();
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var offset = e.NewStartingIndex;
				foreach (var item in e.NewItems)
				{
					Children.Insert(offset++, GetItemView(item));
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (var item in e.OldItems)
				{
					var itemToBeRemoved = Children.FirstOrDefault(x => x.BindingContext == item);
					if (itemToBeRemoved != null)
					{
						Children.Remove(itemToBeRemoved);
					}
				}
			}
		}

		public void SetCollectionListeningOn()
		{
			if (ItemsSource is INotifyCollectionChanged)
			{
				var observable = ItemsSource as INotifyCollectionChanged;
				observable.CollectionChanged -= OnCollectionChanged;
				observable.CollectionChanged += OnCollectionChanged;
			}
		}

		public void SetCollectionListeningOff()
		{
			if (ItemsSource is INotifyCollectionChanged)
			{
				var observable = ItemsSource as INotifyCollectionChanged;
				observable.CollectionChanged -= OnCollectionChanged;
			}
		}

		public void SetItems()
		{
			Children.Clear();

			if (ItemsSource == null)
				return;

			foreach (var item in ItemsSource)
				Children.Add(GetItemView(item));

			SelectedItem = null;
		}

		private View GetItemView(object item)
		{
			var content = ItemTemplate.CreateContent();
			var view = content as View;
			view.BindingContext = item;

			view.GestureRecognizers.Add(new TapGestureRecognizer((sender, obj) => onItemTapped(sender, item)));

			return view;
		}

		void onItemTapped(View sender, object obj)
		{
			var item = sender.BindingContext;
			if (ItemTappedCommand != null)
			{
				ItemTappedCommand.Execute(item);
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var itemsView = (StackList)bindable;

			if (oldValue == null)
				return;

			if (newValue == oldValue)
				return;


			var handler = itemsView.SelectedItemChanged;
			if (handler != null)
				handler(itemsView, EventArgs.Empty);
		}
	}
}
