using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Artemkv.Darwin.Controller
{
	public class Commands
	{
		public static RoutedUICommand OpenProjectCommand = InitializeCommand(Key.O, ModifierKeys.Control, "Ctrl+O", "Open Project", "OpenProject");
		public static RoutedUICommand CreateNewProjectCommand = InitializeCommand(Key.N, ModifierKeys.Control, "Ctrl+N", "New Project", "NewProject");
		public static RoutedUICommand CreateNewDatabaseCommand = InitializeCommand(Key.D1, ModifierKeys.Control, "Ctrl-1", "New Database", "NewDatabase");
		public static RoutedUICommand CreateNewEntityCommand = InitializeCommand(Key.D2, ModifierKeys.Control, "Ctrl-2", "New Entity", "NewEntity");
		public static RoutedUICommand CreateNewRelationCommand = InitializeCommand(Key.D3, ModifierKeys.Control, "Ctrl-3", "New Relation", "NewRelation");
		public static RoutedUICommand CreateNewAttributeCommand = InitializeCommand(Key.D4, ModifierKeys.Control, "Ctrl-4", "New Attribute", "NewAttribute");
		public static RoutedUICommand CreateNewBaseEnumCommand = InitializeCommand(Key.D5, ModifierKeys.Control, "Ctrl-5", "New Enum", "NewBaseEnum");
		public static RoutedUICommand CreateNewBaseEnumValueCommand = InitializeCommand(Key.D6, ModifierKeys.Control, "Ctrl-6", "New Enum Value", "NewBaseEnumValue");
		public static RoutedUICommand CreateNewDataTypeCommand = InitializeCommand(Key.D7, ModifierKeys.Control, "Ctrl-7", "New Data Type", "NewDataType");
		public static RoutedUICommand CreateNewDiagramCommand = InitializeCommand(Key.M, ModifierKeys.Control, "Ctrl-M", "New Diagram", "NewDiagram");
		
		public static RoutedUICommand AddEntitiesToDiagramCommand = InitializeCommand(Key.Y, ModifierKeys.Control, "Ctrl+Y", "Add Entities", "AddEntities");
		public static RoutedUICommand ImportObjectsFromDatabaseCommand = InitializeCommand(Key.I, ModifierKeys.Control, "Ctrl+I", "Import", "Import");

		public static RoutedUICommand ChangeObjectCommand = InitializeCommand(Key.E, ModifierKeys.Control, "Ctrl+E", "Edit", "EditObject");
		public static RoutedUICommand AcceptChangesCommand = InitializeCommand(Key.S, ModifierKeys.Control, "Ctrl+S", "OK", "AcceptChanges");
		public static RoutedUICommand CancelChangesCommand = InitializeCommand(Key.Escape, ModifierKeys.None, "Esc", "Cancel", "CancelChanges");

		public static RoutedUICommand DeleteObjectCommand = InitializeCommand(Key.Delete, ModifierKeys.None, "Ctrl+D", "Delete", "Delete");

		public static RoutedUICommand EditDiagramObjectCommand;
		public static RoutedUICommand AddNewEntityOnDiagramCommand;
		public static RoutedUICommand EditEntityCommand;

		private static RoutedUICommand InitializeCommand(Key key, ModifierKeys modifierKeys, string keyDisplayString, string commandText, string commandName)
		{
			InputGestureCollection inputs = new InputGestureCollection();
			inputs.Add(new KeyGesture(key, modifierKeys, keyDisplayString));
			return new RoutedUICommand(commandText, commandName, typeof(Commands), inputs);
		}
	}
}