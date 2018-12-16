using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Filters
{
	[Serializable]
	public class EntitiesNotOnDiagramFilter : ListFilter
	{
		/// <summary>
		/// Required for WCF.
		/// </summary>
		private EntitiesNotOnDiagramFilter()
		{
		}

		public EntitiesNotOnDiagramFilter(Guid databaseID, Guid diagramID)
		{
			if (databaseID != Guid.Empty)
			{
				this.DatabaseID = databaseID;
				this.Add(new ListFilterParameter("Database.ID", DatabaseID));
			}
			if (diagramID != Guid.Empty)
			{
				this.DiagramID = diagramID;
				this.Add(new ListFilterParameter("Diagram.ID", diagramID));
			}
		}

		public Guid DatabaseID { get; private set; }
		public Guid DiagramID { get; private set; }
	}
}
