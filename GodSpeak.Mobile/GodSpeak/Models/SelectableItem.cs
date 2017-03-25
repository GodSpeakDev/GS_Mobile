using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class SelectableItem<T> : MvxViewModel
	{
		public T Item
		{
			get;
			set;
		}

		private bool _isEnabled;
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { SetProperty(ref _isEnabled, value); }
		}
	}
}
