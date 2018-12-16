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
	public class AttributeDTO : PersistentObjectDTO
	{
		public AttributeDTO()
		{
		}

		[ParentObject(typeof(Entity))]
		public Guid EntityID { get; set; }
		
		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string AttributeName { get; set; }

		[ObjectProperty(typeof(DataType))]
		[NotNullValidationRule]
		public Guid DataTypeID { get; set; }

		[SimpleProperty]
		public int Length { get; set; }

		[SimpleProperty]
		public bool IsRequired { get; set; }

		[SimpleProperty]
		public bool IsPrimaryKey { get; set; }

		public override Type PersistentType
		{
			get { return typeof(ERModel.Attribute); }
		}

		public override string ToString()
		{
			return AttributeName;
		}
	}
}
