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
	public class DataTypeDTO : PersistentObjectDTO
	{
		public DataTypeDTO()
		{
		}

		[ParentObject(typeof(Database))]
		public Guid DatabaseID { get; set; }

		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string TypeName { get; set; }

		[SimpleProperty]
		public bool HasLength { get; set; }

		[ObjectProperty(typeof(BaseEnum))]
		public Guid BaseEnumID { get; set; }

		public override Type PersistentType
		{
			get { return typeof(DataType); }
		}

		public override string ToString()
		{
			return TypeName;
		}
	}
}
