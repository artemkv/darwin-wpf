using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	/// <summary>
	/// The entity attribute.
	/// </summary>
	[Serializable]
	public class Attribute : PersistentObject
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		public virtual Entity Entity { get; set; }

		/// <summary>
		/// Gets or sets the attribute name.
		/// </summary>
		public virtual string AttributeName { get; set; }

		/// <summary>
		/// Gets or sets the attribute type.
		/// </summary>
		public virtual DataType DataType { get; set; }

		/// <summary>
		/// Gets or sets the attribute length.
		/// Only attributes of certain attribute types may have length. 
		/// For the sake of simplicity, this property will be ignored if not applicable.
		/// </summary>
		public virtual int Length { get; set; }

		/// <summary>
		/// Gets or sets the value which specifies whether the attribute is required.
		/// </summary>
		public virtual bool IsRequired { get; set; }

		/// <summary>
		/// Gets or sets the value which specifies whether the attribute is a part of a primary key.
		/// </summary>
		public virtual bool IsPrimaryKey { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return AttributeName;
		}

		#endregion Public Methods
	}
}
