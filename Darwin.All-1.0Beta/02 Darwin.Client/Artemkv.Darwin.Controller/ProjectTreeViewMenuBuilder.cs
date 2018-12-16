using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Artemkv.Darwin.Common.TreePaths.ProjectTreePath;
using System.ComponentModel;

namespace Artemkv.Darwin.Controller
{
	internal class ProjectTreeViewMenuBuilder : INotifyPropertyChanged
	{
		#region Private Members

		private ProjectTreeHelper _treeHelper = new ProjectTreeHelper();

		#endregion Private Members

		#region Constructors

		public ProjectTreeViewMenuBuilder()
		{
			if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
				var serviceLocator = ServiceLocator.GetActive();
				var projectTreeView = serviceLocator.SessionState.ProjectTreeView;
				if (projectTreeView == null)
					throw new InvalidOperationException("ProjectTreeViewMenuBuilder is used as a static resource in ProjectTreeView and cannot be created without one.");

				projectTreeView.SelectedItemChanged += projectTreeView_SelectedItemChanged;
			}
		}

		#endregion Constructors

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Events

		#region Public Methods

		public Visibility CreateNewDatabase_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null && node.Path.Leaf == Element.Project)
				{
					return Visibility.Visible;
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewEntity_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Database || node.Path.Leaf == Element.Folder_Entities)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewRelation_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Entity || node.Path.Leaf == Element.Folder_EntityRelations)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewDiagram_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Database || node.Path.Leaf == Element.Folder_Diagrams)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility DeleteObject_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf != Element.Folder_BaseEnums && 
						node.Path.Leaf != Element.Folder_DataTypes &&
						node.Path.Leaf != Element.Folder_Diagrams &&
						node.Path.Leaf != Element.Folder_Entities &&
						node.Path.Leaf != Element.Folder_EntityAttributes &&
						node.Path.Leaf != Element.Folder_EntityRelations)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility AddEntitiesToDiagram_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Diagram)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewAttribute_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Entity || node.Path.Leaf == Element.Folder_EntityAttributes)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewBaseEnum_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Database || node.Path.Leaf == Element.Folder_BaseEnums)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewBaseEnumValue_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.BaseEnum)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		public Visibility CreateNewDataType_Visibility
		{
			get
			{
				var node = _treeHelper.GetCurrentNode();
				if (node != null)
				{
					if (node.Path.Leaf == Element.Database || node.Path.Leaf == Element.Folder_DataTypes)
					{
						return Visibility.Visible;
					}
				}
				return Visibility.Collapsed;
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, e);
			}
		}

		private void projectTreeView_SelectedItemChanged(object sender, EventArgs e)
		{
			// Invalidate all properties
			OnPropertyChanged(new PropertyChangedEventArgs(null));
		}

		#endregion Private Methods
	}
}
