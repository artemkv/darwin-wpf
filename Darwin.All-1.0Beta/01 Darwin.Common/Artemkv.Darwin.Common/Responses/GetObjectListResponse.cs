using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common.Responses
{
	[Serializable]
	public class GetObjectListResponse : ResponseBase
	{
		public GetObjectListResponse(IList<PersistentObjectDTO> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			this.Items = items;
		}

		public IList<PersistentObjectDTO> Items { get; private set; }
		public int ItemsTotal { get; set; }
	}
}
