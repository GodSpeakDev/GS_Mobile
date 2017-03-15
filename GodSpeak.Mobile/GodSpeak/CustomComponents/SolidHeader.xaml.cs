using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GodSpeak
{
	public partial class SolidHeader : Grid
	{
		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create<SolidHeader, string>(
				p => p.Title, string.Empty, BindingMode.TwoWay, propertyChanged: OnTitleChanged);

		public string Title
		{
			get { return (string)this.GetValue(TitleProperty); }
			set { 
				this.SetValue(TitleProperty, value);
				Label.Text = value;
			}
		}

		private static void OnTitleChanged(BindableObject bindable, string oldvalue, string newValue)
		{			
			var header = (SolidHeader)bindable;
			header.Title = newValue;
		}

		public SolidHeader()
		{			
			InitializeComponent();
		}
	}
}
