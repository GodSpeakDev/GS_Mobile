using System;
using Xamarin.Forms;

namespace GodSpeak
{
    public class SharePageSelector : DataTemplateSelector
    {
        private readonly DataTemplate _shareTemplate;
        private readonly DataTemplate _didYouKnowTemplate;

        private readonly DataTemplate _donateTemplate;


        public SharePageSelector ()
        {
            _shareTemplate = new DataTemplate (typeof (ShareTemplateView));
            _donateTemplate = new DataTemplate (typeof (DonateTemplateView));
            _didYouKnowTemplate = new DataTemplate (typeof (DidYouKnowTemplateView));
        }

        protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
        {
            if (item is ShareTemplateViewModel) {
                return _shareTemplate;
            } else if (item is DidYouKnowTemplateViewModel) {
                return _didYouKnowTemplate;
            } else if (item is DonateTemplateViewModel) {
                return _donateTemplate;
            }

            return null;
        }
    }
}
