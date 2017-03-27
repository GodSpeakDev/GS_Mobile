using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GodSpeak.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(ViewCell), typeof(TransparentViewCellRenderer))]
namespace GodSpeak.iOS
{
	public class TransparentViewCellRenderer : ViewCellRenderer
	{
		public TransparentViewCellRenderer()
		{

		}

		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			if (cell != null) cell.BackgroundColor = UIColor.Clear;
			return cell;
		}
	}
}
