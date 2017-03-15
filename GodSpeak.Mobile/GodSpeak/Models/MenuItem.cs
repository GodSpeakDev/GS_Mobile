using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class MenuItem
	{
		public string Image { get; set; }
		public string Title { get; set; }
		public MvxCommand Command { get; set; }
	}
}
