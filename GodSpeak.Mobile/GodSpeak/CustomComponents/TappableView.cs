using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace GodSpeak
{
	public class TappableView : ContentView
	{
		public static readonly BindableProperty CommandProperty =
				BindableProperty.Create<TappableView, ICommand>(
					p => p.Command, null, BindingMode.TwoWay, propertyChanged: OnCommandChanged);

		public ICommand Command
		{
			get { return (ICommand)this.GetValue(CommandProperty); }
			set { 
				this.SetValue(CommandProperty, value);
				GestureRecognizers.Clear();
				GestureRecognizers.Add(new TapGestureRecognizer()
				{
					Command = new Command(() =>
					{
						AnimateView(this, value);
					})
				});
			}
		}

		private static void OnCommandChanged(BindableObject bindable, ICommand oldvalue, ICommand newValue)
		{
			var view = (TappableView)bindable;
			view.Command = newValue;
		}

		private void AnimateView(View view, ICommand command)
		{
			var reduceOpacityAnimation = new Animation((x) =>
			{
				view.Opacity = 1 - x * .5;
			}, finished: () =>
			{
				command.Execute(null);
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

