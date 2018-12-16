using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Artemkv.Darwin.ERModel
{
	/// <summary>
	/// The base class for all persistent objects.
	/// </summary>
	[Serializable]
	public abstract class PersistentObject
	{
		/// <summary>
		/// Gets or sets the object id.
		/// </summary>
		public virtual Guid ID { get; set; }

		/// <summary>
		/// Gets or sets the object version timestamp.
		/// </summary>
		public virtual Byte[] TS { get; set; }
	}
}
