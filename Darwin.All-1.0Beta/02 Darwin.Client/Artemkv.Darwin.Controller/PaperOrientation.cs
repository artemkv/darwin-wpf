using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Controller
{
	public class PaperOrientation
	{
		private static List<PaperOrientation> Orientations = new List<PaperOrientation>()
		{
			new PaperOrientation() { IsVertical = false, Orientation = "Horisontal" },
			new PaperOrientation() { IsVertical = true, Orientation = "Vertical" }
		};

		public bool IsVertical { get; private set; }
		public string Orientation { get; private set; }

		public override string ToString()
		{
			return Orientation;
		}

		public static List<PaperOrientation> GetOrientations()
		{
			return Orientations;
		}
	}
}
