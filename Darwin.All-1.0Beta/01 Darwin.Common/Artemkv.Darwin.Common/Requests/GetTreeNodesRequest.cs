using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Requests
{
	[Serializable]
	public class GetTreeNodesRequest : RequestBase, IPageableRequest
	{
		public GetTreeNodesRequest(string dataSource, Guid parentID, string path)
		{
			if (String.IsNullOrWhiteSpace(dataSource))
				throw new ArgumentNullException("dataSource");
			if (parentID == Guid.Empty)
				throw new ArgumentNullException("parentID");
			if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException("path");

			this.DataSource = dataSource;
			this.ParentID = parentID;
			this.Path = path;
		}

		public string DataSource { get; private set; }
		public Guid ParentID { get; private set; }
		public string Path { get; private set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
