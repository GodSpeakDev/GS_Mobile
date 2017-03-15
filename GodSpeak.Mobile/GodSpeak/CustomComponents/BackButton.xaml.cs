using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;

namespace GodSpeak
{
	public partial class BackButton : StackLayout
	{
		public BackButton()
		{
			InitializeComponent();
			this.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command(() => 
				{
					AnimateView(this, (this.BindingContext as CustomViewModel).CloseCommand, null);
				})
			});
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
