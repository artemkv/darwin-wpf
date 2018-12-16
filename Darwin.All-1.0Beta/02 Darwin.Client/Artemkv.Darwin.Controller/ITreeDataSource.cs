using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller
{
	public interface ITreeDataSource
	{
		ObjectTreeNode GetRootNode(Guid id);
		IList<ObjectTreeNode> GetNodes(ObjectTreeNode parent);
	}
}
