using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Controller
{
	public class PaperSize
	{
		private static List<PaperSize> Sizes = new List<PaperSize>()
		{
			new PaperSize() { Size = 1, Name = "A1", Height = 33.1, Width = 23.4 },
			new PaperSize() { Size = 2, Name = "A2", Height = 23.4, Width = 16.5 },
			new PaperSize() { Size = 3, Name = "A3", Height = 16.5, Width = 11.7 },
			new PaperSize() { Size = 4, Name = "A4", Height = 11.7, Width = 8.3 },
			new PaperSize() { Size = 5, Name = "A5", Height = 8.3, Width = 5.8 }
		};

		public static readonly int A1 = 1;
		public static readonly int A2 = 2;
		public static readonly int A3 = 3;
		public static readonly int A4 = 4;
		public static readonly int A5 = 5;

		public int Size { get; private set; }
		public string Name { get; private set; }

		public double Width { get; private set; }
		public double Height { get; private set; }

		public override string ToString()
		{
			return Name;
		}

		public static List<PaperSize> GetSizes()
		{
			return Sizes;
		}

		public static double GetHeight(int size, bool isVertical)
		{
			if (size < 1 || size > 5)
				throw new ArgumentOutOfRangeException(String.Format("Size {0} is not supported", size));

			return isVertical ? Sizes[size - 1].Height : Sizes[size - 1].Width;
		}

		public static double GetWidth(int size, bool isVertical)
		{
			if (size < 1 || size > 5)
				throw new ArgumentOutOfRangeException(String.Format("Size {0} is not supported", size));

			return isVertical ? Sizes[size - 1].Width : Sizes[size - 1].Height;
		}
	}
}
