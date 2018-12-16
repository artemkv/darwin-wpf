using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Globalization;
using System.Windows;

namespace Artemkv.Darwin.Visual.Primitives
{
	[Serializable]
	public class TextRectangle : Primitive
	{
		#region Class Memebers

		private string _text = String.Empty;
		private double _emSize = 14.00;
		private double _padding = 4.00;
		private FormattedText _formattedText;
		private double _minWidth = 128.00;
		private double _minHeight = 16.00;

		#endregion Class Memebers

		#region .Ctor

		public TextRectangle()
		{
			Initialize();
		}

		#endregion .Ctor

		#region Internal Properties

		internal override double OwnWidth
		{
			get
			{
				// Own width is at least a width of the text.

				if (String.IsNullOrWhiteSpace(_formattedText.Text))
				{
					return base.OwnWidth;
				}

				double width = Math.Round(_formattedText.Width + Padding * 2.00);
				return width > MinWidth ? width : MinWidth;
			}
		}

		internal override double OwnHeight
		{
			get
			{
				// Own height is at least a height of the text.

				if (String.IsNullOrWhiteSpace(_formattedText.Text))
				{
					return base.OwnHeight;
				}

				double height = _formattedText.Height + Padding * 2.00;
				return height > MinHeight ? height : MinHeight;
			}
		}

		#endregion Internal Properties

		#region Public Properties

		public override double X
		{
			get
			{
				if (Container != null)
				{
					return Container.GetItemX(this);
				}
				return base.X;
			}
			set
			{
				base.X = value;
			}
		}

		public override double Y
		{
			get
			{
				if (Container != null)
				{
					return Container.GetItemY(this);
				}
				return base.Y;
			}
			set
			{
				base.Y = value;
			}
		}

		public override double Width
		{
			get
			{
				if (Container != null)
				{
					return Container.Width;
				}

				return OwnWidth;
			}
			set
			{
				base.Width = value;
			}
		}

		public override double Height
		{
			get
			{
				return OwnHeight;
			}
			set
			{
				base.Height = value;
			}
		}

		public string Text
		{
			get { return _text; }
			set
			{
				_text = value; 
				Initialize();
			}
		}

		public double EmSize
		{
			get { return _emSize; }
			set { _emSize = value; }
		}

		public double Padding
		{
			get { return _padding; }
			set { _padding = value; }
		}

		public double MinWidth
		{
			get { return _minWidth; }
			set { _minWidth = value; }
		}

		public double MinHeight
		{
			get { return _minHeight; }
			set { _minHeight = value; }
		}

		#endregion Public Properties

		#region Private Members

		private void Initialize()
		{
			var typeface = new Typeface(new FontFamily(), new FontStyle(), new FontWeight(), new FontStretch());

			_formattedText = new FormattedText(
				_text,
				CultureInfo.InvariantCulture,
				FlowDirection.LeftToRight,
				typeface,
				_emSize,
				Brushes.Black);
			_formattedText.MaxLineCount = 1;
		}

		#endregion Private Members
	}
}
