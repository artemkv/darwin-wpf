using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Filters
{
	[Serializable]
	public class DatabaseEntitiesFilter : ListFilter
	{
		/// <summary>
		/// Required for WCF.
		/// </summary>
		private DatabaseEntitiesFilter()
		{
		}

		public DatabaseEntitiesFilter(Guid databaseID)
		{
			if (databaseID != Guid.Empty)
			{
				this.DatabaseID = databaseID;
				this.Add(new ListFilterParameter("Database.ID", DatabaseID));
			}
		}

		public Guid DatabaseID { get; private set; }
	}
}
