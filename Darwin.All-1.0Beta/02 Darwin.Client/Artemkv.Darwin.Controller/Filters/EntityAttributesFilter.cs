using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Filters
{
	[Serializable]
	public class EntityAttributesFilter : ListFilter
	{
		/// <summary>
		/// Required for WCF.
		/// </summary>
		private EntityAttributesFilter()
		{
		}

		public EntityAttributesFilter(Guid entityID)
		{
			if (entityID != Guid.Empty)
			{
				this.EntityID = entityID;
				this.Add(new ListFilterParameter("Entity.ID", EntityID));
			}
		}

		public Guid EntityID { get; private set; }
	}
}
