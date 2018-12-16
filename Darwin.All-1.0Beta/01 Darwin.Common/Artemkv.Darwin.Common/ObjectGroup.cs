using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common
{
	/// <summary>
	/// Represents the group of objects in the tree.
	/// </summary>
	[Serializable]
	public class ObjectGroup
	{
		/// <summary>
		/// Creates an instance of <c>ObjectGroup</c> class.
		/// </summary>
		/// <param name="title">Object group title.</param>
		public ObjectGroup(string title)
		{
			this.Title = title;
		}

		/// <summary>
		/// Gets or sets the object group title.
		/// </summary>
		public string Title { get; private set; }

		public override string ToString()
		{
			return Title;
		}
	}
}
