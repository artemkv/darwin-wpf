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
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using Artemkv.Darwin.Common.Validation;
using Artemkv.Darwin.Common.DTO;

namespace Darwin
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.SessionState.TreePanel = TreePanel;
			serviceLocator.SessionState.DetailsPanel = DetailsPanel;
			serviceLocator.SessionState.BusyIndicator = AppBusyIndicator;
			serviceLocator.BasicController.ProcessStartNewSession();

			ErrorsGrid.ItemsSource = serviceLocator.SessionState.ValidationErrors;

			PersistentObjectDTO.Validated += PersistentObjectDTO_Validated;
		}

		#endregion Constructors

		#region Event Handlers

		void PersistentObjectDTO_Validated(object sender, ObjectValidationEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();

			if (e.IsValid)
			{
				serviceLocator.SessionState.ValidationErrors.Remove(e.Details);
			}
			else
			{
				if (!serviceLocator.SessionState.ValidationErrors.Contains(e.Details))
				{
					serviceLocator.SessionState.ValidationErrors.Add(e.Details);
				}
			}
		}

		#endregion Event Handlers

		#region Basic Actions

		private void DeleteObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessDeleteObject();
		}

		private void DeleteObjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.BasicController.CanDeleteObject;
		}

		private void AcceptChangesCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessSaveChanges();
		}

		private void AcceptChangesCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.BasicController.CanSaveChanges;
		}

		private void CancelChangesCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessCancelChanges();
		}

		private void CancelChangesCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.BasicController.CanCancelChanges;
		}

		private void ChangeObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessChangeObject();
		}

		private void ChangeObjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.BasicController.CanChangeObject;
		}

		#endregion Basic Actions

		#region Project

		private void OpenProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ProjectController.ProcessOpenProject();
			}
		}

		private void CreateNewProjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ProjectController.ProcessCreateNewProject();
			}
		}

		#endregion Project

		#region Model

		private void CreateNewDatabaseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewDatabase();
			}
		}

		private void CreateNewEntityCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewEntity();
			}
		}

		private void CreateNewBaseEnumCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewBaseEnum();
			}
		}

		private void CreateNewBaseEnumValueCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewBaseEnumValue();
			}
		}

		private void CreateNewDataTypeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewDataType();
			}
		}

		private void CreateNewRelationCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewRelation();
			}
		}

		private void CreateNewAttributeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewAttribute();
			}
		}

		private void CreateNewDiagramCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			if (serviceLocator.BasicController.CanNavigateAway())
			{
				serviceLocator.ModelController.ProcessCreateNewDiagram();
			}
		}

		private void AddEntitiesToDiagramCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessSaveChanges(continueEditing: true);
			serviceLocator.ModelController.ProcessAddEntitiesToDiagram();
		}

		private void CreateNewDatabaseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewDatabase;
		}

		private void CreateNewEntityCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewEntity;
		}

		private void CreateNewBaseEnumCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewBaseEnum;
		}

		private void CreateNewBaseEnumValueCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewBaseEnumValue;
		}

		private void CreateNewDataTypeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewDataType;
		}

		private void CreateNewRelationCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewRelation;
		}

		private void CreateNewAttributeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewAttribute;
		}

		private void CreateNewDiagramCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanCreateNewDiagram;
		}

		private void AddEntitiesToDiagramCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ModelController.CanAddEntitiesToDiagram;
		}

		#endregion Model

		#region Import

		private void ImportObjectsFromDatabaseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.ImportController.ProcessImportObjectsFromDatabase();
		}

		private void ImportObjectsFromDatabaseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			e.CanExecute = serviceLocator.ImportController.CanImportObjectsFromDatabase;
		}

		#endregion Import
	}
}