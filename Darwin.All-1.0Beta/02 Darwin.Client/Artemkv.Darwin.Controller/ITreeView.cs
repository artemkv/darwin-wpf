using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Controller
{
	public interface ITreeView
	{
		IList<ObjectTreeNode> DataSource { get; set; }
	}
}
