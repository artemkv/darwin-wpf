using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Visual.Primitives
{
	[Serializable]
	public class VerticalContainer : Container
	{
		#region Public Properties

		public override double Height
		{
			get
			{
				// The container height is at least the total height of all items in the container.

				double itemsHeightTotal = 0.00;
				foreach (Primitive item in Items)
				{
					itemsHeightTotal += item.OwnHeight;
				}

				if (itemsHeightTotal > base.Height)
				{
					return itemsHeightTotal;
				}

				return base.Height;
			}
			set
			{
				base.Height = value;
			}
		}

		public override double Width
		{
			get
			{
				// The container width is at least the width of the longest item in the container.

				double itemWidthMaximum = 0.00;
				foreach (Primitive item in Items)
				{
					if (item.OwnWidth > itemWidthMaximum)
					{
						itemWidthMaximum = item.OwnWidth;
					}
				}

				if (itemWidthMaximum > base.Width)
				{
					return itemWidthMaximum;
				}

				return base.Width;
			}
			set
			{
				base.Width = value;
			}
		}

		#endregion Public Properties

		#region Internal Methods

		internal override double GetItemX(Primitive item)
		{
			return X;
		}

		internal override double GetItemY(Primitive item)
		{
			double y = this.Y;
			foreach (Primitive currentItem in Items)
			{
				if (currentItem == item)
				{
					break;
				}
				y += currentItem.Height;
			}

			return y;
		}

		#endregion Internal Methods
	}
}
