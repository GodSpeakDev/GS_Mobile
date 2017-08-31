using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class MenuItem : MvxViewModel
	{
		public enum HintModes
		{
			External,
			Inline,
		}

		public HintModes HintMode
		{
			get;
			set;
		}

		private string _image;
		public string Image
		{
			get
			{
				return _image;
			}
			set
			{
				SetProperty(ref _image, value);
			}
		}

		public string ShownImage
		{
			get
			{
				if (IsHighlighted)
				{
					return Image;
				}
				else
				{
					return _image.Replace(".png", "_unselected.png");
				}
			}
		}

		private bool _isHighlighted = true;
		public bool IsHighlighted
		{
			get
			{
				return _isHighlighted;
			}
			set
			{
				SetProperty(ref _isHighlighted, value);
				RaisePropertyChanged(() => Image);
				RaisePropertyChanged(() => ShownImage);
			}
		}

		private bool _showHint = false;
		public bool ShowHint
		{
			get
			{
				return _showHint;
			}
			set
			{
				SetProperty(ref _showHint, value);
				RaisePropertyChanged(() => ShownTitle);
			}
		}

		private string _hint;
		public string Hint
		{
			get { return _hint; }
			set { SetProperty(ref _hint, value); }
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		public string ShownTitle
		{
			get
			{
				if (ShowHint)
				{
					return HintMode == HintModes.Inline ? Hint : string.Empty;
				}
				else
				{
					return Title;
				}
			}
		}

		public MvxCommand Command { get; set; }
	}
}
