using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.TreePaths.ProjectTreePath;
using Artemkv.Darwin.Common.Requests;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Artemkv.Darwin.Controller.DataSources
{
	/// <summary>
	/// The project tree data source.
	/// </summary>
	public class ProjectTreeDataSource : ITreeDataSource
	{
		public ObjectTreeNode GetRootNode(Guid id)
		{
			var project = new ObjectDataSource().GetObject(ObjectType.Project, id);
			var node = new ObjectTreeNode(null, project, project.ID, this, TreeNodePath.Root.Then(Element.Project));
			return node;

		}

		public IList<ObjectTreeNode> GetNodes(ObjectTreeNode parent)
		{
			var request = new GetTreeNodesRequest(
				ObjectTreeDataSource.ProjectTreeView,
				parent.ObjectID,
				parent.Path);

			using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
			{
				return DataSourceHelper.CombinePages<ObjectTreeNode>(
					r => from x in proxy.GetTreeNodes(r as GetTreeNodesRequest).Nodes
						 select new ObjectTreeNode(
							 parent,
							 x.Object,
							 x.ObjectID,
							 this,
							 parent.Path.Then(x.SubPath),
							 x.IsLeaf),
					request).ToList<ObjectTreeNode>();
			}
		}
	}
}
