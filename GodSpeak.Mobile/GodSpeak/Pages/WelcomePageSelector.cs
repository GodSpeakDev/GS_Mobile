using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class WelcomePageSelector : DataTemplateSelector
	{
		private readonly DataTemplate _getStartedTemplate;
		private readonly DataTemplate _claimInviteCodeTemplate;
		private readonly DataTemplate _requestInviteCodeTemplate;

		public WelcomePageSelector()
		{
			_getStartedTemplate = new DataTemplate(typeof(GetStartedPage));
			_claimInviteCodeTemplate = new DataTemplate(typeof(ClaimInviteCodeView));
			_requestInviteCodeTemplate = new DataTemplate(typeof(RequestInviteCodeView));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item is GetStartedViewModel)
			{
				return _getStartedTemplate;
			}
			else if (item is ClaimInviteCodeViewModel)
			{
				return _claimInviteCodeTemplate;
			}
			else if (item is RequestInviteCodeViewModel)
			{
				return _requestInviteCodeTemplate;
			}

			return null;
		}
	}
}
