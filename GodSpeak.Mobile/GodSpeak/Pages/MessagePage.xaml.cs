using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GodSpeak.Extensions;

namespace GodSpeak
{
    public partial class MessagePage : ContentPage
    {
        public MessagePage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);
            MessagesListView.DeselectOnTap ();
        }
    }
}
