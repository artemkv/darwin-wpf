using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common.Responses
{
	[Serializable]
	public class GetTreeNodesResponse : ResponseBase
	{
		public GetTreeNodesResponse(IList<TreeNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException("nodes");

			this.Nodes = nodes;
		}

		public IList<TreeNode> Nodes { get; private set; }
	}
}
