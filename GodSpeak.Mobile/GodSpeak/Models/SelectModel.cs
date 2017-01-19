using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class SelectModel<T>
	{		
		public MvxCommand<SelectModel<T>> Command
		{
			get;
			set;
		}

		public T Model
		{
			get;
			set;
		}
	}
}
