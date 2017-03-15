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

		public static readonly BindableProperty HasBackButtonProperty =
			BindableProperty.Create<SolidHeader, bool>(
				p => p.HasBackButton, true, BindingMode.TwoWay, propertyChanged: HasBackButtonChanged);

		public bool HasBackButton
		{
			get { return (bool)this.GetValue(HasBackButtonProperty); }
			set
			{
				this.SetValue(HasBackButtonProperty, value);
				BackButton.IsVisible = HasBackButton;
			}
		}

		private static void HasBackButtonChanged(BindableObject bindable, bool oldvalue, bool newValue)
		{
			var header = (SolidHeader)bindable;
			header.HasBackButton = newValue;
		}

		public SolidHeader()
		{			
			InitializeComponent();
		}
	}
}
