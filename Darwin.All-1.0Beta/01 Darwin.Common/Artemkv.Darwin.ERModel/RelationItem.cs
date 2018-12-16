using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	/// <summary>
	/// The entity.
	/// </summary>
	[Serializable]
	public class RelationItem : PersistentObject
	{
		#region Public Properties

		/// <summary>
		/// The relation.
		/// </summary>
		public virtual Relation Relation { get; set; }

		/// <summary>
		/// The attribute on the primary key side.
		/// </summary>
		public virtual ERModel.Attribute PrimaryAttribute { get; set; }

		/// <summary>
		/// The attribute on the foreign key side.
		/// </summary>
		public virtual ERModel.Attribute ForeignAttribute { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return ForeignAttribute.AttributeName + " >- " + PrimaryAttribute.AttributeName;
		}

		#endregion Public Methods
	}
}
