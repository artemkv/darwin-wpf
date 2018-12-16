using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Filters
{
	[Serializable]
	public class DiagramRelationsFilter : ListFilter
	{
		/// <summary>
		/// Required for WCF.
		/// </summary>
		private DiagramRelationsFilter()
		{
		}

		public DiagramRelationsFilter(Guid diagramID)
		{
			if (diagramID != Guid.Empty)
			{
				this.DiagramID = diagramID;
				this.Add(new ListFilterParameter("DiagramID", diagramID));
			}
		}

		public Guid DiagramID { get; private set; }
	}
}
