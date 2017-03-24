using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace GodSpeak
{
    public static class ViewExtensions
    {
        public static void DeselectOnTap (this ListView listView)
        {
            listView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
                listView.SelectedItem = null;
            };
        }

		public static void AnimateViewWhenTap(this View view, ICommand command, object commandParameter)
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
