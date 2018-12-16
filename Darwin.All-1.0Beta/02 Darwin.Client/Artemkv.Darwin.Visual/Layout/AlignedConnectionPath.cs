using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Visual.Layout
{
	/// <summary>
	/// Represents a connection path aligned with other paths which begin or end at the same side.
	/// </summary>
	public class AlignedConnectionPath
	{
		/// <summary>
		/// The original connection path.
		/// </summary>
		public ConnectionPath ConnectionPath { get; set; }

		/// <summary>
		/// Index of the path end coming from the side A.
		/// </summary>
		public int PathEndAtSideAIndex { get; set; }

		/// <summary>
		/// Total number of path ends coming from the side A.
		/// </summary>
		public int PathEndAtSideATotal { get; set; }

		/// <summary>
		/// Index of the path end coming to the side B.
		/// </summary>
		public int PathEndAtSideBIndex { get; set; }

		/// <summary>
		/// Total number of path ends coming to the side B.
		/// </summary>
		public int PathEndAtSideBTotal { get; set; }
	}
}
