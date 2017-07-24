using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;

namespace GodSpeak
{
	public partial class MenuItemView : ContentView
	{
		public static readonly BindableProperty SourceProperty =
			BindableProperty.Create<MenuItemView, string>(
				p => p.Source, null, BindingMode.TwoWay, propertyChanged: OnSourceChanged);

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create<MenuItemView, ICommand>(
				p => p.Command, null, BindingMode.TwoWay, propertyChanged: OnCommandChanged);

		public static readonly BindableProperty SizeProperty =
			BindableProperty.Create<MenuItemView, double>(
				p => p.Size, -1, BindingMode.TwoWay, propertyChanged: OnSizeChanged);

		public string Source
		{
			get { return (string)this.GetValue(SourceProperty); }
			set
			{
				this.SetValue(SourceProperty, value);
				Image.Source = value;
			}
		}

		public double Size
		{
			get { return (double)this.GetValue(SizeProperty); }
			set
			{
				this.SetValue(SizeProperty, value);
				Image.WidthRequest = value;
				Image.HeightRequest = value;
			}
		}

		public ICommand Command
		{
			get { return (ICommand)this.GetValue(CommandProperty); }
			set
			{
				this.SetValue(CommandProperty, value);
				this.GestureRecognizers.Clear();
				this.GestureRecognizers.Add(new TapGestureRecognizer()
				{
					Command = new Command(() => AnimateView(this, value, null))
				});
			}
		}

		private static void OnCommandChanged(BindableObject bindable, ICommand oldvalue, ICommand newValue)
		{
			var entry = (MenuItemView)bindable;
			entry.Command = newValue;
		}

		private static void OnSourceChanged(BindableObject bindable, string oldvalue, string newValue)
		{
			var element = (MenuItemView)bindable;
			element.Source = newValue;
		}

		private static void OnSizeChanged(BindableObject bindable, double oldvalue, double newValue)
		{
			var element = (MenuItemView)bindable;
			element.Size = newValue;
		}

		public MenuItemView()
		{
			InitializeComponent();
		}

		private void AnimateView(View view, ICommand command, object commandParameter)
		{			
			var reduceOpacityAnimation = new Animation((x) =>
			{
				view.Opacity = 1 - x * .5;
			}, finished: () =>
			{
				command.Execute(commandParameter);
			});

			var increaseOpacityAnimation = new Animation((x) =>
			{
				view.Opacity = 1 - 0.5 + x * .5;
			});

			var animation = new Animation();
			animation.Add(0, 0.5, reduceOpacityAnimation);
			animation.Add(0.5, 1, increaseOpacityAnimation);
			animation.Commit(view, "Tap");
		}
	}
}
