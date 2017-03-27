using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Linq;
using GodSpeak.iOS;

[assembly: ResolutionGroupName("GodSpeak")]
[assembly: ExportEffect(typeof(ReturnKeyEffect), "ReturnKeyEffect")]
namespace GodSpeak.iOS
{
	public class ReturnKeyEffect : PlatformEffect
	{
		public ReturnKeyEffect()
		{
		}

		protected override void OnAttached()
		{
			try
			{
				var effect = (GodSpeak.ReturnKeyEffect)Element.Effects.FirstOrDefault(e => e is GodSpeak.ReturnKeyEffect);
				if (effect != null)
				{
					if (effect.ReturnText == "Done")
						(Control as UITextField).ReturnKeyType = UIReturnKeyType.Done;
					if (effect.ReturnText == "Next")
						(Control as UITextField).ReturnKeyType = UIReturnKeyType.Next;
				}
			}
			catch
			{
				Console.WriteLine(string.Format("Can't apply effect to this control type of {0}", Control.GetType().Name));
			}
		}


		#region implemented abstract members of Effect

		protected override void OnDetached()
		{
			//do nothing
		}

		#endregion
	}
}
