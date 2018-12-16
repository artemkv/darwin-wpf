using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common
{
	/// <summary>
	/// The list filter. Is used to pass the filtering parameters for the server list data source.
	/// </summary>
	[Serializable]
	public class ListFilter : List<ListFilterParameter>
	{
		/// <summary>
		/// Returns an empty filter.
		/// </summary>
		public static ListFilter Empty
		{
			get
			{
				return new ListFilter();
			}
		}
	}
}
