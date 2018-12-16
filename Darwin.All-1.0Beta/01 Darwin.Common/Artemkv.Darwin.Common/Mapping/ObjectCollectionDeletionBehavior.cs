using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Mapping
{
	public enum ObjectCollectionDeletionBehavior
	{
		/// <summary>
		/// Delete object completely. Default behavior.
		/// </summary>
		Delete,
		/// <summary>
		/// Detach object from the collection, but do not delete completely.
		/// </summary>
		DetachOnly
	}
}
