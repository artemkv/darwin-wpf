using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Controller.Views;
using Artemkv.Darwin.Controller.DataSources;
using Artemkv.Darwin.Common;
using System.Windows;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Controller.Areas
{
	public class BasicController
	{
		public void ProcessStartNewSession()
		{
			var serviceLocator = ServiceLocator.GetActive();
			var view = new ProjectTreeView();
			serviceLocator.SessionState.ProjectTreeView = view;
			serviceLocator.SessionState.TreePanel.Children.Add(view);

			serviceLocator.ProjectController.ProcessOpenProject();
		}

		public void ProcessShowProjectTreeNodeDetails()
		{
			var serviceLocator = ServiceLocator.GetActive();
			serviceLocator.SessionState.ValidationErrors.Clear();
			serviceLocator.SessionState.ObjectInEditor = null;

			var node = new ProjectTreeHelper().GetCurrentNode();
			if (node == null)
			{
				serviceLocator.SessionState.DetailsPanel.Children.Clear();
				serviceLocator.SessionState.DetailsView = null;
				return;
			}

			IDetailsView view = null;
			var persistentObject = node.Object as PersistentObjectDTO;
			if (persistentObject != null)
			{
				if (persistentObject.PersistentType == typeof(Entity))
				{
					view = new EntityDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(Project))
				{
					view = new ProjectDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(Database))
				{
					view = new DatabaseDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(ERModel.Attribute))
				{
					view = new AttributeDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(DataType))
				{
					view = new DataTypeDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(Relation))
				{
					view = new RelationDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(RelationItem))
				{
					view = new RelationItemDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(Diagram))
				{
					view = new DiagramChartView();
				}
				else if (persistentObject.PersistentType == typeof(DiagramEntity))
				{
					view = new DiagramEntityDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(BaseEnum))
				{
					view = new BaseEnumDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(BaseEnumValue))
				{
					view = new BaseEnumValueDetailsView();
				}

				if (view != null)
				{
					view.Object = persistentObject;
				}
			}

			serviceLocator.SessionState.DetailsPanel.Children.Clear();
			serviceLocator.SessionState.DetailsView = view;
			serviceLocator.SessionState.ObjectInEditor = persistentObject;

			if (view != null)
			{
				serviceLocator.SessionState.DetailsPanel.Children.Add(view as UIElement);
			}
		}

		public void ProcessProjectTreeRefresh()
		{
			var node = new ProjectTreeHelper().GetCurrentNode();
			if (node == null)
			{
				return;
			}

			node.Refresh();
			ProcessShowProjectTreeNodeDetails();
		}

		public void ProcessSaveChanges(bool continueEditing = false)
		{
			var serviceLocator = ServiceLocator.GetActive();
			var obj = serviceLocator.SessionState.ObjectInEditor;
			if (obj == null)
				throw new InvalidOperationException("Nothing to save");

			try
			{
				new ObjectDataSource().SaveObject(obj);
				if (!continueEditing)
				{
					serviceLocator.SessionState.ObjectChanged = false;
				}
				serviceLocator.BasicController.ProcessProjectTreeRefresh();

				// TODO: this is a bugfix for "Refresh current item after changes have been applied" - verify it is indeed a fix, then uncomment
				// ProcessProjectTreeRefresh();
			}
			catch (StaleObjectStateException)
			{
				// TODO: this doesn't catch StaleObjectStateException, because it is replaced by WCF.
				// TODO: Should be application error.
				throw;
			}
		}

		public void ProcessDeleteObject()
		{
			if (MessageBox.Show("Delete object?", "Delete", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
			{
				return;
			}

			var node = new ProjectTreeHelper().GetCurrentNode();
			if (node == null)
			{
				return;
			}

			var parent = node.Parent;

			var obj = node.Object as PersistentObjectDTO;
			if (obj != null)
			{
				new ObjectDataSource().DeleteObject(obj);
				// TODO: refresh parent
			}

			if (parent != null)
			{
				parent.Refresh();
			}
		}

		public bool CanDeleteObject
		{
			get
			{
				var node = new ProjectTreeHelper().GetCurrentNode();
				if (node == null)
				{
					return false;
				}
				var obj = node.Object as PersistentObjectDTO;
				if (obj == null)
				{
					return false;
				}
				return true;
			}
		}

		public bool CanSaveChanges
		{
			get
			{
				var serviceLocator = ServiceLocator.GetActive();

				var obj = serviceLocator.SessionState.ObjectInEditor;
				if (obj == null || new Validator().Validate(obj) != null)
				{
					return false;
				}

				return serviceLocator.SessionState.ObjectChanged;
			}
		}

		public void ProcessCancelChanges()
		{
			var serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessProjectTreeRefresh();
			serviceLocator.SessionState.ObjectChanged = false;
		}

		public bool CanCancelChanges
		{
			get
			{
				var serviceLocator = ServiceLocator.GetActive();
				return serviceLocator.SessionState.ObjectChanged;
			}
		}

		public void ProcessChangeObject()
		{
			var serviceLocator = ServiceLocator.GetActive();
			this.ProcessProjectTreeRefresh();
			serviceLocator.SessionState.ObjectChanged = true;
		}

		public bool CanChangeObject
		{
			get
			{
				var serviceLocator = ServiceLocator.GetActive();
				return !serviceLocator.SessionState.ObjectChanged;
			}
		}

		public bool CanNavigateAway()
		{
			var serviceLocator = ServiceLocator.GetActive();
			if (!serviceLocator.SessionState.ObjectChanged)
			{
				return true;
			}
			var dialogResult = MessageBox.Show("Changes has not been saved. Do you want to save you changes before proceed?", "Darwin", MessageBoxButton.YesNoCancel);
			if (dialogResult == MessageBoxResult.Yes)
			{
				this.ProcessSaveChanges();
				return true;
			}
			if (dialogResult == MessageBoxResult.No)
			{
				this.ProcessCancelChanges();
				return true;
			}
			if (dialogResult == MessageBoxResult.Cancel)
			{
				return false;
			}
			return false;
		}
	}
}