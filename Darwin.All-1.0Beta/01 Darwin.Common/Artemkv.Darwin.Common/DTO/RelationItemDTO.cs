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
	public class RelationItemDTO : PersistentObjectDTO
	{
		public RelationItemDTO()
		{
		}

		[ParentObject(typeof(Relation))]
		public Guid RelationID { get; set; }

		[ObjectProperty(typeof(ERModel.Attribute))]
		[NotNullValidationRule]
		public Guid PrimaryAttributeID { get; set; }
		[ObjectProperty(typeof(ERModel.Attribute))]
		[NotNullValidationRule]
		public Guid ForeignAttributeID { get; set; }

		[CalculatedProperty("Relation.PrimaryEntity.EntityName")]
		public string PrimaryEntityName { get; set; }
		[CalculatedProperty("Relation.ForeignEntity.EntityName")]
		public string ForeignEntityName { get; set; }
		[CalculatedProperty("PrimaryAttribute.AttributeName")]
		public string PrimaryAttributeName { get; set; }
		[CalculatedProperty("ForeignAttribute.AttributeName")]
		public string ForeignAttributeName { get; set; }
		[CalculatedProperty("ForeignAttribute.IsRequired")]
		public bool ForeignAttributeRequired { get; set; }

		public override Type PersistentType
		{
			get { return typeof(RelationItem); }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(ForeignEntityName);
			sb.Append(".");
			sb.Append(ForeignAttributeName);
			sb.Append(" >- ");
			sb.Append(PrimaryEntityName);
			sb.Append(".");
			sb.Append(PrimaryAttributeName);

			return sb.ToString();
		}
	}
}
