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
	public class BaseEnumValue : PersistentObject
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the base enum.
		/// </summary>
		public virtual BaseEnum BaseEnum { get; set; }

		/// <summary>
		/// Gets or sets the base enum value name.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Gets or sets the base enum value integer value.
		/// </summary>
		public virtual int Value { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return String.Format("{0} ({1})", Name, Value);
		}

		#endregion Public Methods
	}
}
