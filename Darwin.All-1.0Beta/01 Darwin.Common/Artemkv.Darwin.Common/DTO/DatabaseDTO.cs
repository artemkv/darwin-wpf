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
	public class DatabaseDTO : PersistentObjectDTO
	{
		private readonly List<DataTypeDTO> _dataTypes = new List<DataTypeDTO>();

		public DatabaseDTO()
		{
		}

		[ParentObject(typeof(Project))]
		public Guid ProjectID { get; set; }

		[SimpleProperty]
		[RegExpValidationRule(ValidationPatterns.DbIdentifierValidationPattern, 
			ValidationPatterns.DbIdentifierValidationPatternDescription)]
		public string DBName { get; set; }
		[SimpleProperty]
		public string ConnectionString { get; set; }

		[ObjectCollection(parentProperty: "Database")]
		public virtual List<DataTypeDTO> DataTypes
		{
			get
			{
				return _dataTypes;
			}
		}

		public override Type PersistentType
		{
			get { return typeof(Database); }
		}

		public override string ToString()
		{
			return DBName;
		}
	}
}
