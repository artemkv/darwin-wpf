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
	public class RelationDTO : PersistentObjectDTO
	{
		private readonly List<RelationItemDTO> _items = new List<RelationItemDTO>();

		public RelationDTO()
		{
		}

		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string RelationName { get; set; }
		[SimpleProperty]
		public bool OneToOne { get; set; }
		[SimpleProperty]
		public bool AtLeastOne { get; set; }

		[ObjectProperty(typeof(Entity))]
		public Guid PrimaryEntityID { get; set; }
		[ObjectProperty(typeof(Entity))]
		public Guid ForeignEntityID { get; set; }

		[ObjectCollection(parentProperty: "Relation")]
		public virtual List<RelationItemDTO> Items
		{
			get
			{
				return _items;
			}
		}

		public override Type PersistentType
		{
			get { return typeof(Relation); }
		}

		public override string ToString()
		{
			return RelationName;
		}
	}
}
