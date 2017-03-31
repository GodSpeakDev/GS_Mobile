using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class ItemCommand<T> : MvxViewModel
	{
		public T Item
		{
			get;
			set;
		}

		private MvxCommand<T> _tappedCommand;
		public MvxCommand<T> TappedCommand
		{
			get { return _tappedCommand; }
			set { 
				_tappedCommand = new MvxCommand<T>((obj) => 
				{ 
					value.Execute(this.Item); 
				}); 
			}
		}
	}
}
