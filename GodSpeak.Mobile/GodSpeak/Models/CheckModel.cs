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
			set { 
				SetProperty(ref _isChecked, value);
				RaisePropertyChanged(nameof(Height));
			}
		}

		private double _height;
		public double Height
		{
			get { return IsChecked ? 50 : 0; }
			set { SetProperty(ref _height, value); }
		}

		public T Model
		{
			get;
			set;
		}
	}
}
