using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GodSpeak
{
	public partial class SettingsCell : ViewCell
	{
		public SettingsCell()
		{
			InitializeComponent();
		}

		void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			var sw = (Switch)sender;
			var grid = (Grid) sw.Parent;

			var checkModel = (CheckModel<DayOfWeekSettings>)sw.BindingContext;
			//grid.HeightRequest = checkModel.IsChecked ? 200 : 10;
			grid.ForceLayout();
			this.ForceUpdateSize();
		}
	}
}
