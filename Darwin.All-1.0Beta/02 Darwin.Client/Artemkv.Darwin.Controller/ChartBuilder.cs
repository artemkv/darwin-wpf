using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;
using System.Xml;
using Artemkv.Darwin.Common.DTO;
using System.Windows.Media;

namespace Artemkv.Darwin.Controller
{
	public class ChartBuilder
	{
		#region Class Members

		private Dictionary<Guid, VerticalContainer> _entities = new Dictionary<Guid, VerticalContainer>();

		private List<Primitive> _items = new List<Primitive>();
		private List<Connector> _connections = new List<Connector>();

		#endregion Class Members

		#region Public Properties

		public List<Primitive> Items
		{
			get
			{
				Primitive[] items = new Primitive[_items.Count];
				_items.CopyTo(items);
				return items.ToList();
			}
		}

		public List<Connector> Connections
		{
			get
			{
				Connector[] connections = new Connector[_connections.Count];
				_connections.CopyTo(connections);
				return connections.ToList();
			}
		}

		#endregion Public Properties

		#region Public Methods

		public void BuildFromDiagram(DiagramDTO diagram)
		{
			if (diagram == null)
				throw new ArgumentNullException("diagram");

			ProcessEntities(diagram);
			ProcessRelations(diagram);
		}

		#endregion Public Methods

		#region Private Methods

		private void ProcessEntities(DiagramDTO diagram)
		{
			foreach (var diagramEntity in diagram.Entities)
			{
				var entityContainer = new VerticalContainer()
				{
					Object = diagramEntity,
					X = diagramEntity.X,
					Y = diagramEntity.Y
				};
				
				// Entity header
				var entityNameBox = new TextRectangle()
				{
					Text = diagramEntity.EntitySchemaPrefixedName,
					BgColor = Colors.LightCyan
				};
				entityContainer.AddItem(diagramEntity.EntitySchemaPrefixedName, entityNameBox);

				// Entity attributes
				foreach (var attr in diagramEntity.Entity.Attributes)
				{
					var attrBox = new TextRectangle()
					{
						Text = attr.AttributeName
					};
					entityContainer.AddItem(attr.AttributeName, attrBox);
				}

				// Sentinel
				var sentinel = new TextRectangle()
				{
					Text = "."
				};
				entityContainer.AddItem("sentinel", sentinel);

				_items.Add(entityContainer);
				_entities.Add(diagramEntity.EntityID, entityContainer); // TODO: should not be possible to add a table to the diagram more than once, otherwise fails here.
			}
		}

		private void ProcessRelations(DiagramDTO diagram)
		{
			var serviceLocator = ServiceLocator.GetActive();
			var relations = serviceLocator.RelationInfo.GetDiagramRelations(diagram.ID);

			foreach (var relation in relations)
			{
				var connector = new Connector()
				{
					From = _entities[relation.ForeignEntityID],
					To = _entities[relation.PrimaryEntityID]
				};

				SetConnectorType(relation, connector);

				_connections.Add(connector);
			}
		}

		private void SetConnectorType(RelationDTO relation, Connector connector)
		{
			// Any of the foreign key fields is nullable - make the parent relation zero to one.
			if (relation.Items.Any(x => !x.ForeignAttributeRequired))
			{
				connector.ToConnectorType = ConnectorType.ZeroOrOne;
			}
			else
			{
				connector.ToConnectorType = ConnectorType.ExactlyOne;
			}

			if (relation.OneToOne)
			{
				if (relation.AtLeastOne)
				{
					connector.FromConnectorType = ConnectorType.ExactlyOne;
				}
				else
				{
					connector.FromConnectorType = ConnectorType.ZeroOrOne;
				}
			}
			else
			{
				if (relation.AtLeastOne)
				{
					connector.FromConnectorType = ConnectorType.OneOrMany;
				}
				else
				{
					connector.FromConnectorType = ConnectorType.ZeroOrMany;
				}
			}
		}

		#endregion Private Methods
	}
}
