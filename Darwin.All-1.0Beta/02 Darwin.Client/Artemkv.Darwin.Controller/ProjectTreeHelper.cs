using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Controller
{
	public class ProjectTreeHelper
	{
		public ObjectTreeNode GetCurrentNode()
		{
			var serviceLocator = ServiceLocator.GetActive();
			var projectTreeView = serviceLocator.SessionState.ProjectTreeView;
			if (projectTreeView == null)
			{
				return null;
			}
			return projectTreeView.SelectedNode as ObjectTreeNode;
		}

		public Guid GetCurrentProjectID()
		{
			var serviceLocator = ServiceLocator.GetActive();
			var projectTreeView = serviceLocator.SessionState.ProjectTreeView;
			if (projectTreeView == null ||
				projectTreeView.DataSource == null ||
				projectTreeView.DataSource.Count == 0)
			{
				return Guid.Empty;
			}
			var node = projectTreeView.DataSource[0] as ObjectTreeNode;
			if (node == null || node.ObjectID == Guid.Empty)
			{
				return Guid.Empty;
			}
			return node.ObjectID;
		}

		public Guid GetFirstAncestorID<T>() where T : class
		{
			var serviceLocator = ServiceLocator.GetActive();
			var projectTreeView = serviceLocator.SessionState.ProjectTreeView;
			if (projectTreeView == null ||
				projectTreeView.DataSource == null ||
				projectTreeView.DataSource.Count == 0)
			{
				return Guid.Empty;
			}
			var node = projectTreeView.SelectedNode as ObjectTreeNode;
			while (node != null)
			{
				if (node.Object is T)
				{
					return node.ObjectID;
				}
				node = node.Parent;
			}
			return Guid.Empty;
		}

		public T GetFirstAncestor<T>() where T : class
		{
			var serviceLocator = ServiceLocator.GetActive();
			var projectTreeView = serviceLocator.SessionState.ProjectTreeView;
			if (projectTreeView == null ||
				projectTreeView.DataSource == null ||
				projectTreeView.DataSource.Count == 0)
			{
				return null;
			}
			var node = projectTreeView.SelectedNode as ObjectTreeNode;
			while (node != null)
			{
				var obj = node.Object as T;
				if (obj != null)
				{
					return obj;
				}
				node = node.Parent;
			}
			return null;
		}
	}
}
