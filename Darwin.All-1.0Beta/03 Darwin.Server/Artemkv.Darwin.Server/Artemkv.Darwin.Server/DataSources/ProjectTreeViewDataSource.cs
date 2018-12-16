using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Data;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.TreePaths.ProjectTreePath;
using Artemkv.Darwin.Common.DTO;
using NHibernate;

namespace Artemkv.Darwin.Server.DataSources
{
	/// <summary>
	/// The datasource for the project tree.
	/// </summary>
	public class ProjectTreeViewDataSource
	{
		public IList<TreeNode> GetNodes(Guid parentId, string path, ISession session, int pageSize, int pageNumber)
		{
			var serviceLocator = ServiceLocator.GetActive();
			var dataProvider = serviceLocator.DataProvider;

			IEnumerable<TreeNode> nodes = null;
			var asm = new Assembler();
			if (path == TreeNodePath.Root)
			{
				var project = asm.GetDTO(dataProvider.GetObject<Project>(parentId, session));
				nodes = new List<TreeNode>() 
				{
					new TreeNode (project, project.ID, Element.Project)
				};
			}
			else if (path == TreeNodePath.Root.Then(Element.Project))
			{
				nodes = from x in dataProvider.GetObjectList<Database>(x => x.Project.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.Database);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database))
			{
				nodes = new List<TreeNode>() 
				{
					new TreeNode (new ObjectGroup("Diagrams"), parentId, Element.Folder_Diagrams), 
					new TreeNode (new ObjectGroup("Enums"), parentId, Element.Folder_BaseEnums), 
					new TreeNode (new ObjectGroup("Data Types"), parentId, Element.Folder_DataTypes), 
					new TreeNode (new ObjectGroup("Entities"), parentId, Element.Folder_Entities)
				};
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_BaseEnums))
			{
				nodes = from x in dataProvider.GetObjectList<BaseEnum>(x => x.Database.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.BaseEnum, isLeaf: false);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_BaseEnums)
								.Then(Element.BaseEnum))
			{
				nodes = from x in dataProvider.GetObjectList<ERModel.BaseEnumValue>(x => x.BaseEnum.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.BaseEnumValue, isLeaf: true);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_DataTypes))
			{
				nodes = from x in dataProvider.GetObjectList<DataType>(x => x.Database.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.DataType, isLeaf: true);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Entities))
			{
				nodes = from x in dataProvider.GetObjectList<Entity>(x => x.Database.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.Entity);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Entities)
								.Then(Element.Entity))
			{
				nodes = new List<TreeNode>() 
				{
					new TreeNode (new ObjectGroup("Attributes"), parentId, Element.Folder_EntityAttributes), 
					new TreeNode (new ObjectGroup("Relations"), parentId, Element.Folder_EntityRelations)
				};
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Entities)
								.Then(Element.Entity)
								.Then(Element.Folder_EntityAttributes))
			{
				nodes = from x in dataProvider.GetObjectList<ERModel.Attribute>(x => x.Entity.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.Attribute, isLeaf: true);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Entities)
								.Then(Element.Entity)
								.Then(Element.Folder_EntityRelations))
			{
				nodes = from x in dataProvider.GetObjectList<Relation>(x => x.ForeignEntity.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.Relation);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Entities)
								.Then(Element.Entity)
								.Then(Element.Folder_EntityRelations)
								.Then(Element.Relation))
			{
				nodes = from x in dataProvider.GetObjectList<RelationItem>(x => x.Relation.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.RelationItem, isLeaf: true);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Diagrams))
			{
				nodes = from x in dataProvider.GetObjectList<Diagram>(x => x.Database.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.Diagram);
			}
			else if (path == TreeNodePath.Root
								.Then(Element.Project)
								.Then(Element.Database)
								.Then(Element.Folder_Diagrams)
								.Then(Element.Diagram))
			{
				nodes = from x in dataProvider.GetObjectList<DiagramEntity>(x => x.Diagram.ID == parentId, session, pageSize, pageNumber).ResultSet
						select new TreeNode(asm.GetDTO(x), x.ID, Element.DiagramEntity, isLeaf: true);
			}
			else
			{
				throw new InvalidOperationException(String.Format("Invalid path: {0}", path));
			}
			return nodes.ToList<TreeNode>();
		}
	}
}
