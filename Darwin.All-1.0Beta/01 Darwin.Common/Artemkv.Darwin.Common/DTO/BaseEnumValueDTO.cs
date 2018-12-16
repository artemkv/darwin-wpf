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
	public class BaseEnumValueDTO : PersistentObjectDTO
	{
		public BaseEnumValueDTO()
		{
		}

		[ParentObject(typeof(BaseEnum))]
		public Guid BaseEnumID { get; set; }

		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string Name { get; set; }

		[SimpleProperty]
		public int Value { get; set; }

		public override Type PersistentType
		{
			get { return typeof(BaseEnumValue); }
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", Name, Value);
		}
	}
}
