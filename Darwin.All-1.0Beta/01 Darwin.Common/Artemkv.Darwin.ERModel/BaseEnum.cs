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
	public class BaseEnum : PersistentObject
	{
		#region Class Members

		private readonly IList<BaseEnumValue> _values = new List<BaseEnumValue>();

		#endregion Class Members

		#region Public Properties

		/// <summary>
		/// Gets or sets the database.
		/// </summary>
		public virtual Database Database { get; set; }

		/// <summary>
		/// Gets or sets the base enum name.
		/// </summary>
		public virtual string BaseEnumName { get; set; }

		/// <summary>
		/// Gets the collection of attributes.
		/// </summary>
		public virtual IList<BaseEnumValue> Values
		{
			get
			{
				return _values;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return BaseEnumName;
		}

		#endregion Public Methods
	}
}
