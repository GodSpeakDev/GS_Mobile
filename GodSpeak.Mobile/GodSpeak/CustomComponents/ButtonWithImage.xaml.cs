using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;

namespace GodSpeak
{
	public partial class ButtonWithImage : StackLayout
	{		
		public ImageSource Image
		{
			get { return Icon.Source;}
			set { Icon.Source = value; }
		}

		public string Text
		{
			get { return Label.Text; }
			set { Label.Text = value; }
		}

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create<ButtonWithImage, ICommand>(
				p => p.Command, null, BindingMode.TwoWay, propertyChanged: OnCommandChanged);

		private ICommand _command;
		public ICommand Command
		{
			get { return _command;}
			set 
			{ 
				_command = value;
				this.GestureRecognizers.Clear();
				this.GestureRecognizers.Add(new TapGestureRecognizer() 
				{ 
					Command = new Command(() => this.AnimateViewWhenTap(_command, null))
				});
			}
		}

		private static void OnCommandChanged(BindableObject bindable, ICommand oldvalue, ICommand newValue)
		{
			var element = (ButtonWithImage)bindable;
			element.Command = newValue;
		}

		public ButtonWithImage()
		{
			InitializeComponent();
		}
	}
}
