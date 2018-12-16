using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Artemkv.Darwin.ERModel;
using System.ComponentModel;
using Artemkv.Darwin.Common.Validation;
using Artemkv.Darwin.Common.Mapping;

namespace Artemkv.Darwin.Common.DTO
{
	[Serializable]
	[KnownType(typeof(ProjectDTO))]
	[KnownType(typeof(DatabaseDTO))]
	[KnownType(typeof(EntityDTO))]
	[KnownType(typeof(DataTypeDTO))]
	[KnownType(typeof(AttributeDTO))]
	[KnownType(typeof(RelationDTO))]
	[KnownType(typeof(RelationItemDTO))]
	[KnownType(typeof(DiagramDTO))]
	[KnownType(typeof(DiagramEntityDTO))]
	[KnownType(typeof(BaseEnumDTO))]
	[KnownType(typeof(BaseEnumValueDTO))]
	public abstract class PersistentObjectDTO : IDataErrorInfo
	{
		public PersistentObjectDTO()
		{
			this.ID = Guid.NewGuid();
		}

		public static event EventHandler<ObjectValidationEventArgs> Validated;

		public abstract Type PersistentType { get; }

		public Guid ID { get; set; }

		[SimpleProperty]
		public virtual Byte[] TS { get; set; }

		public string Error
		{
			get { return "Invalid property value."; }
		}

		/// <summary>
		/// Used for built-in WPF validation.
		/// Additionally, raises Validated event so application can react on validation appropriately.
		/// Standart Validation.Error event is quirky in Data Grids 
		/// and it's behaviour has been subject to breaking changes when switching from .net 3.5 to 4.0.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public string this[string propertyName]
		{
			get
			{
				var error = new Validator().Validate(this, propertyName);
				string errorMessage = error == null ? null : error.ToString();

				// Fire Validated event.
				var validationDetails = new ObjectPropertyValidationDetails(PersistentType, ID, propertyName) { ErrorMessage = errorMessage };
				var validationEventArgs = new ObjectValidationEventArgs(errorMessage == null, validationDetails);
				var tmpValidated = Validated;
				if (tmpValidated != null)
				{
					tmpValidated(this, validationEventArgs);
				}

				return errorMessage;
			}
		}
	}
}
