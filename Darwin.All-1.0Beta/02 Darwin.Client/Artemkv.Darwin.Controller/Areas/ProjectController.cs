using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Controller.Views;
using Artemkv.Darwin.Controller.Windows;
using Artemkv.Darwin.Controller.DataSources;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Requests;

namespace Artemkv.Darwin.Controller.Areas
{
	public class ProjectController
	{
		public void ProcessOpenProject()
		{
			var serviceLocator = ServiceLocator.GetActive();

			var view = new ProjectListView()
			{
				DataSource = new ProjectListDataSource()
			};

			var popup = new PopupWindow();
			popup.Title = "Projects";
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				var item = view.SelectedItem as ObjectListItem;
				if (item != null && item.PersistentObject != null)
				{
					var treeDataSource = new ProjectTreeDataSource();
					var node = treeDataSource.GetRootNode(item.PersistentObject.ID);
					var nodes = new List<ObjectTreeNode>() { node };
					serviceLocator.SessionState.ProjectTreeView.DataSource = nodes;
				}
			}
		}

		public void ProcessCreateNewProject()
		{
			var newProject = new ProjectDTO() { ProjectName = "<Enter name>" };

			var view = new ProjectDetailsView();
			view.Object = newProject;

			var popup = new PopupWindow();
			popup.Title = "New Project";
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
				{
					proxy.SaveObject(new SaveObjectRequest(newProject));
				}
			}
		}
	}
}
