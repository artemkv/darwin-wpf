using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Filters
{
	[Serializable]
	public class DiagramEntitiesFilter : ListFilter
	{
		/// <summary>
		/// Required for WCF.
		/// </summary>
		private DiagramEntitiesFilter()
		{
		}

		public DiagramEntitiesFilter(Guid diagramID)
		{
			if (diagramID != Guid.Empty)
			{
				this.DiagramID = diagramID;
				this.Add(new ListFilterParameter("Diagram.ID", diagramID));
			}
		}

		public Guid DiagramID { get; private set; }
	}
}
