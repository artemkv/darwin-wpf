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
	public class BaseEnumDTO : PersistentObjectDTO
	{
		private readonly List<BaseEnumValueDTO> _values = new List<BaseEnumValueDTO>();

		public BaseEnumDTO()
		{
		}

		[ParentObject(typeof(Database))]
		public Guid DatabaseID { get; set; }

		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string BaseEnumName { get; set; }

		[ObjectCollection(parentProperty: "BaseEnum")]
		public virtual List<BaseEnumValueDTO> Values
		{
			get
			{
				return _values;
			}
		}

		public override Type PersistentType
		{
			get { return typeof(BaseEnum); }
		}

		public override string ToString()
		{
			return BaseEnumName;
		}
	}
}
