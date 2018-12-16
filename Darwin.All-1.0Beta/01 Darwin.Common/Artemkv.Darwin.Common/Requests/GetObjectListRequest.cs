using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Requests
{
	[Serializable]
	public class GetObjectListRequest : RequestBase, IPageableRequest
	{
		public GetObjectListRequest(string dataSource)
		{
			if (String.IsNullOrWhiteSpace(dataSource))
				throw new ArgumentNullException("dataSource");

			this.DataSource = dataSource;
		}

		public string DataSource { get; private set; }

		public ListFilter ListFilter { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
