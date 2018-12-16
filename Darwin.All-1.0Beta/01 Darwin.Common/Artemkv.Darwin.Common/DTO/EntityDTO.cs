using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.Mapping;
using Artemkv.Darwin.Common.Validation;

namespace Artemkv.Darwin.Common.DTO
{
	[Serializable]
	public class EntityDTO : PersistentObjectDTO
	{
		private readonly List<AttributeDTO> _attributes = new List<AttributeDTO>();

		public EntityDTO()
		{
		}

		[ParentObject(typeof(Database))]
		public Guid DatabaseID { get; set; }

		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string SchemaName { get; set; }
		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string EntityName { get; set; }

		[ObjectCollection(parentProperty: "Entity")]
		public virtual List<AttributeDTO> Attributes
		{
			get
			{
				return _attributes;
			}
		}

		public string EntitySchemaPrefixedName
		{
			get
			{
				return SchemaName + "." + EntityName;
			}
		}

		public override Type PersistentType
		{
			get { return typeof(Entity); }
		}

		public override string ToString()
		{
			return EntitySchemaPrefixedName;
		}
	}
}
