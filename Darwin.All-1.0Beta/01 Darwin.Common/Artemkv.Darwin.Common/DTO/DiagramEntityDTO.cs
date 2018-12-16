using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.Mapping;

namespace Artemkv.Darwin.Common.DTO
{
	[Serializable]
	public class DiagramEntityDTO : PersistentObjectDTO
	{
		public DiagramEntityDTO()
		{
		}

		[ParentObject(typeof(Diagram))]
		public Guid DiagramID { get; set; }

		[ObjectProperty(typeof(Entity))]
		public Guid EntityID { get; set; }

		[ObjectViewProperty(typeof(Entity))]
		public EntityDTO Entity { get; set; }

		[CalculatedProperty("Entity.EntityName")]
		public string EntityName { get; set; }
		[CalculatedProperty("Entity.SchemaName")]
		public string SchemaName { get; set; }

		[SimpleProperty]
		public int X { get; set; }
		[SimpleProperty]
		public int Y { get; set; }

		public override Type PersistentType
		{
			get { return typeof(DiagramEntity); }
		}

		public virtual string EntitySchemaPrefixedName
		{
			get
			{
				return SchemaName + "." + EntityName;
			}
		}

		public override string ToString()
		{
			return EntitySchemaPrefixedName;
		}
	}
}
