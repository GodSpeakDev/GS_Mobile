using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace GodSpeak
{
	public class ImageButton : Image
	{
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create<ImageButton, ICommand>(
				p => p.Command, null, BindingMode.TwoWay, propertyChanged: OnCommandChanged);

		public ICommand Command
		{
			get { return (ICommand)this.GetValue(CommandProperty); }
			set { 
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
			var entry = (ImageButton)bindable;
			entry.Command = newValue;
		}

		public static readonly BindableProperty PreCommandProperty =
			BindableProperty.Create<ImageButton, ICommand>(
				p => p.PreCommand, null, BindingMode.TwoWay, propertyChanged: OnPreCommandChanged);

		public ICommand PreCommand
		{
			get { return (ICommand)this.GetValue(PreCommandProperty); }
			set
			{
				this.SetValue(PreCommandProperty, value);
			}
		}

		private static void OnPreCommandChanged(BindableObject bindable, ICommand oldvalue, ICommand newValue)
		{
			var entry = (ImageButton)bindable;
			entry.PreCommand = newValue;
		}

		public ImageButton()
		{
		}

		private void AnimateView(View view, ICommand command, object commandParameter)
		{
			if (PreCommand != null)
			{
				PreCommand.Execute(null);
			}

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
