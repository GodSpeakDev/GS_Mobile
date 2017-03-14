using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class SharePageSelector : DataTemplateSelector
	{
		private readonly DataTemplate _shareTemplate;
		private readonly DataTemplate _didYouKnowTemplate;

		public SharePageSelector()
		{
			_shareTemplate = new DataTemplate(typeof(ShareTemplateView));
			_didYouKnowTemplate = new DataTemplate(typeof(DidYouKnowTemplateView));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item is ShareTemplateViewModel)
			{
				return _shareTemplate;
			}
			else if (item is DidYouKnowTemplateViewModel)
			{
				return _didYouKnowTemplate;
			}

			return null;
		}
	}
}
