using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	/// <summary>
	/// The data type.
	/// </summary>
	[Serializable]
	public class DataType : PersistentObject
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the database.
		/// </summary>
		public virtual Database Database { get; set; }

		/// <summary>
		/// Gets or sets the type name.
		/// </summary>
		public virtual string TypeName { get; set; }

		/// <summary>
		/// Gets or sets the value which defines whether the type can have length.
		/// </summary>
		public virtual bool HasLength { get; set; }

		/// <summary>
		/// Gets or sets the type base enum.
		/// </summary>
		public virtual BaseEnum BaseEnum { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return TypeName;
		}

		#endregion Public Methods
	}
}
