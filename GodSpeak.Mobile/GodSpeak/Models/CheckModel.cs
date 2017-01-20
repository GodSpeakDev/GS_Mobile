using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class CheckModel<T> : MvxViewModel
	{
		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked;}
			set { SetProperty(ref _isChecked, value);}
		}

		public T Model
		{
			get;
			set;
		}
	}
}
