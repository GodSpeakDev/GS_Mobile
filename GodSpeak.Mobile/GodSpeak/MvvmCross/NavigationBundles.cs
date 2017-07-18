using System;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;

namespace GodSpeak
{
	public static class NavigationBundles
	{
		public static MvxBundle ClearStackBundle
		{
			get 
			{
				return new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "ClearStack" } });	
			}
		}

		public static MvxBundle RestoreNavigationBundle
		{
			get
			{
				return new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "RestoreNavigation" } });
			}
		}
	}
}
