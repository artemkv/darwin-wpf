using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Controller.Views;
using Artemkv.Darwin.Controller.Windows;
using Artemkv.Darwin.Controller.DataSources;
using System.Windows;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Areas
{
	public class ModelController
	{
		public void ProcessCreateNewDatabase()
		{
			var projectID = new ProjectTreeHelper().GetCurrentProjectID();
			if (projectID == Guid.Empty)
			{
				throw new InvalidOperationException("No active project.");
			}

			// TODO: move to separate class dedicated to the object creation
			var newDatabase = new DatabaseDTO() { DBName = "<Enter name>", ProjectID = projectID };
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Boolean" });

			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Int8" });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Int16" });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Int32" });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Int64" });

			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Real32" });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Real64" });

			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Char", HasLength = true });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "NChar", HasLength = true });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "VarChar", HasLength = true });
			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "NVarChar", HasLength = true });

			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "Guid" });

			newDatabase.DataTypes.Add(new DataTypeDTO() { DatabaseID = newDatabase.ID, TypeName = "DateTime" });

			var view = new DatabaseDetailsView();
			view.Object = newDatabase;

			var popup = new PopupWindow();
			popup.Title = "New Database";
			popup.Validate = () => { return new Validator().Validate(newDatabase); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newDatabase);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewDatabase
		{
			get
			{
				var projectID = new ProjectTreeHelper().GetCurrentProjectID();
				if (projectID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public EntityDTO ProcessCreateNewEntity()
		{
			var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
			if (databaseID == Guid.Empty)
			{
				throw new InvalidOperationException("No database selected.");
			}
			var newEntity = new EntityDTO() { EntityName = "<Enter name>", SchemaName = "dbo", DatabaseID = databaseID };

			var view = new EntityDetailsView();
			view.Object = newEntity;

			var popup = new PopupWindow();
			popup.Title = "New Entity";
			popup.Validate = () => { return new Validator().Validate(newEntity); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() != true)
			{
				return null;
			}

			new ObjectDataSource().SaveObject(newEntity);
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessProjectTreeRefresh();
			return newEntity;
		}

		public bool CanCreateNewEntity
		{
			get
			{
				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessCreateNewAttribute()
		{
			var entityID = new ProjectTreeHelper().GetFirstAncestorID<EntityDTO>();
			if (entityID == Guid.Empty)
			{
				throw new InvalidOperationException("No entity selected.");
			}
			var newAttribute = new AttributeDTO() { AttributeName = "<Enter name>", EntityID = entityID };

			var view = new AttributeDetailsView();
			view.Object = newAttribute;

			var popup = new PopupWindow();
			popup.Title = "New Attribute";
			popup.Validate = () => { return new Validator().Validate(newAttribute); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newAttribute);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewAttribute
		{
			get
			{
				var entityID = new ProjectTreeHelper().GetFirstAncestorID<EntityDTO>();
				if (entityID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessCreateNewBaseEnum()
		{
			var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
			if (databaseID == Guid.Empty)
			{
				throw new InvalidOperationException("No database selected.");
			}
			var newBaseEnum = new BaseEnumDTO() { BaseEnumName = "<Enter name>", DatabaseID = databaseID };

			var view = new BaseEnumDetailsView();
			view.Object = newBaseEnum;

			var popup = new PopupWindow();
			popup.Title = "New Enum";
			popup.Validate = () => { return new Validator().Validate(newBaseEnum); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newBaseEnum);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewBaseEnum
		{
			get
			{
				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessCreateNewBaseEnumValue()
		{
			var baseEnumID = new ProjectTreeHelper().GetFirstAncestorID<BaseEnumDTO>();
			if (baseEnumID == Guid.Empty)
			{
				throw new InvalidOperationException("No enum selected.");
			}
			var newBaseEnumValue = new BaseEnumValueDTO() { Name = "<Enter name>", BaseEnumID = baseEnumID };

			var view = new BaseEnumValueDetailsView();
			view.Object = newBaseEnumValue;

			var popup = new PopupWindow();
			popup.Title = "New Enum Value";
			popup.Validate = () => { return new Validator().Validate(newBaseEnumValue); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newBaseEnumValue);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewBaseEnumValue
		{
			get
			{
				var baseEnumID = new ProjectTreeHelper().GetFirstAncestorID<BaseEnumDTO>();
				if (baseEnumID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessCreateNewDataType()
		{
			var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
			if (databaseID == Guid.Empty)
			{
				throw new InvalidOperationException("No database selected.");
			}
			var newDataType = new DataTypeDTO() { TypeName = "<Enter name>", DatabaseID = databaseID };

			var view = new DataTypeDetailsView();
			view.Object = newDataType;

			var popup = new PopupWindow();
			popup.Title = "New Data Type";
			popup.Validate = () => { return new Validator().Validate(newDataType); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newDataType);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewDataType
		{
			get
			{
				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessCreateNewDiagram()
		{
			var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
			if (databaseID == Guid.Empty)
			{
				throw new InvalidOperationException("No database selected.");
			}
			var newDiagram = new DiagramDTO() { DiagramName = "<Enter name>", DatabaseID = databaseID };

			var view = new DiagramDetailsView();
			view.Object = newDiagram;

			var popup = new PopupWindow();
			popup.Title = "New Diagram";
			popup.Validate = () => { return new Validator().Validate(newDiagram); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newDiagram);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewDiagram
		{
			get
			{
				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessAddEntitiesToDiagram()
		{
			var diagramID = new ProjectTreeHelper().GetFirstAncestorID<DiagramDTO>();
			if (diagramID == Guid.Empty)
			{
				throw new InvalidOperationException("No diagram selected.");
			}

			var view = new EntitiesListView();
			view.DataSource = new EntitiesNotOnDiagramListDataSource();
			var popup = new PopupWindow();
			popup.Title = "Add Entity To Diagram";
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				var selectedItems = view.SelectedItems;
				if (selectedItems != null)
				{
					foreach (var item in selectedItems)
					{
						new ObjectDataSource().SaveObject(
							new DiagramEntityDTO()
							{
								DiagramID = diagramID,
								EntityID = item.PersistentObject.ID
							});
					}
				}
			}

			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessProjectTreeRefresh();
		}

		public bool CanAddEntitiesToDiagram
		{
			get
			{
				var diagramID = new ProjectTreeHelper().GetFirstAncestorID<DiagramDTO>();
				if (diagramID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessCreateNewRelation()
		{
			var entityID = new ProjectTreeHelper().GetFirstAncestorID<EntityDTO>();
			if (entityID == Guid.Empty)
			{
				throw new InvalidOperationException("No entity selected.");
			}
			var newRelation = new RelationDTO() { RelationName = "<Enter name>", ForeignEntityID = entityID };

			var view = new RelationDetailsView();
			view.Object = newRelation;

			var popup = new PopupWindow();
			popup.Title = "New Relation";
			popup.Validate = () => { return new Validator().Validate(newRelation); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newRelation);
				ServiceLocator serviceLocator = ServiceLocator.GetActive();
				serviceLocator.BasicController.ProcessProjectTreeRefresh();
			}
		}

		public bool CanCreateNewRelation
		{
			get
			{
				var entityID = new ProjectTreeHelper().GetFirstAncestorID<EntityDTO>();
				if (entityID == Guid.Empty)
				{
					return false;
				}
				return true;
			}
		}

		public void ProcessEditDiagramObject(PersistentObjectDTO obj)
		{
			UIElement view = null;
			var persistentObject = obj as PersistentObjectDTO;
			if (persistentObject != null)
			{
				if (persistentObject.PersistentType == typeof(Entity))
				{
					view = new EntityDetailsView();
				}
				else if (persistentObject.PersistentType == typeof(Diagram))
				{
					view = new DiagramDetailsView();
				}

				if (view != null)
				{
					((IDetailsView)view).Object = persistentObject;
				}
			}

			if (view != null)
			{
				var popup = new PopupWindow();
				popup.Title = "Edit " + persistentObject.PersistentType.Name;
				popup.Validate = () => { return new Validator().Validate(persistentObject); };
				popup.ViewPanel.Children.Add(view);

				if (popup.ShowDialog() == true)
				{
					new ObjectDataSource().SaveObject(obj);
					ServiceLocator serviceLocator = ServiceLocator.GetActive();
					serviceLocator.BasicController.ProcessProjectTreeRefresh();
				}
			}
		}

		public void ProcessAddNewEntityOnDiagram(DiagramDTO diagram, Point point)
		{
			if (diagram == null)
			{
				throw new InvalidOperationException("No diagram selected.");
			}

			var newEntity = ProcessCreateNewEntity();
			if (newEntity != null)
			{
				var newDiagramEntity = new DiagramEntityDTO()
				{
					DiagramID = diagram.ID,
					EntityID = newEntity.ID,
					X = (int)point.X,
					Y = (int)point.Y
				};
				new ObjectDataSource().SaveObject(newDiagramEntity);
			}
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessProjectTreeRefresh();
		}

		public void ProcessEditEntity(EntityDTO entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
			if (databaseID == Guid.Empty)
			{
				throw new InvalidOperationException("No database selected.");
			}

			var view = new EntityDetailsView();
			view.Object = entity;

			var popup = new PopupWindow();
			popup.Title = "Edit Entity";
			popup.Validate = () => { return new Validator().Validate(entity); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(entity);
				// Do not refresh node, since can be called from RelationDetailsView
			}
		}

		public void ProcessConnectEntitiesOnDiagram(DiagramEntityDTO from, DiagramEntityDTO to)
		{
			if (from == null)
				throw new ArgumentNullException("from");
			if (to == null)
				throw new ArgumentNullException("to");

			var newRelation = new RelationDTO()
			{
				RelationName = String.Format("{0}_{1}", from.EntityName, to.EntityName),
				PrimaryEntityID = to.EntityID,
				ForeignEntityID = from.EntityID
			};

			var view = new RelationDetailsView();
			view.Object = newRelation;

			var popup = new PopupWindow();
			popup.Title = "New Relation";
			popup.Validate = () => { return new Validator().Validate(newRelation); };
			popup.ViewPanel.Children.Add(view);

			if (popup.ShowDialog() == true)
			{
				new ObjectDataSource().SaveObject(newRelation);
			}
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.BasicController.ProcessProjectTreeRefresh();
		}
	}
}
