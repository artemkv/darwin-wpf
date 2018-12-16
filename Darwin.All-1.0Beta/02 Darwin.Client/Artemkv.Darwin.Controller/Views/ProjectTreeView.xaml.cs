using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Artemkv.Darwin.Controller;
using System.Collections;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.TreePaths.ProjectTreePath;
using System.ComponentModel;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for ProjectTreeView.xaml
	/// </summary>
	public partial class ProjectTreeView : UserControl, ITreeView
	{
		#region Constructors

		public ProjectTreeView()
		{
			InitializeComponent();
		}

		#endregion Constructors

		public event EventHandler<EventArgs> SelectedItemChanged;

		#region Properties

		public IList<ObjectTreeNode> DataSource
		{
			get
			{
				return ProjectTree.ItemsSource as IList<ObjectTreeNode>;
			}
			set
			{
				ProjectTree.ItemsSource = value;
			}
		}

		public object SelectedNode
		{
			get
			{
				return ProjectTree.SelectedItem;
			}
		}

		#endregion Properties

		#region Event Handlers

		private void ProjectTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.SessionState.ObjectChanged)
			{
				serviceLocator.BasicController.ProcessSaveChanges();
			}
			serviceLocator.BasicController.ProcessShowProjectTreeNodeDetails();

			if (SelectedItemChanged != null)
			{
				SelectedItemChanged(this, e);
			}
		}

		private void ProjectTree_Expanded(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = e.OriginalSource as TreeViewItem;

			if (item != null)
			{
				var node = item.Header as ObjectTreeNode;
				if (node != null)
				{
					node.OnExpand();
				}
			}
		}

		private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem item = sender as TreeViewItem;
			if (item != null)
			{
				item.IsSelected = true;
			}
		}

		#endregion Event Handlers

		#region Command Event Handlers

		private void OnRefresh(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessProjectTreeRefresh();
		}

		#endregion Command Event Handlers
	}
}
