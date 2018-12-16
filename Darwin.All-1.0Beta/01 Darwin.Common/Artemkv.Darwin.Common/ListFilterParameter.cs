using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common
{
	/// <summary>
	/// The filter parameter.
	/// </summary>
	[Serializable]
	public class ListFilterParameter
	{
		private ListFilterParameter(string property, object value)
		{
			if (String.IsNullOrWhiteSpace(property))
				throw new ArgumentNullException("property");

			this.Property = property;
			this.Value = value;
		}

		/// <summary>
		/// Crates a filter parameter.
		/// </summary>
		/// <param name="property">The property to filter by.</param>
		/// <param name="value">The constant value to filter by.</param>
		public ListFilterParameter(string property, string value)
			: this(property, (object)value)
		{
		}

		/// <summary>
		/// Crates a filter parameter.
		/// </summary>
		/// <param name="property">The property to filter by.</param>
		/// <param name="value">The constant value to filter by.</param>
		public ListFilterParameter(string property, Guid value)
			: this(property, (object)value)
		{
		}

		/// <summary>
		/// Crates a filter parameter.
		/// </summary>
		/// <param name="property">The property to filter by.</param>
		/// <param name="value">The constant value to filter by.</param>
		public ListFilterParameter(string property, Int64 value)
			: this(property, (object)value)
		{
		}

		/// <summary>
		/// Crates a filter parameter.
		/// </summary>
		/// <param name="property">The property to filter by.</param>
		/// <param name="value">The constant value to filter by.</param>
		public ListFilterParameter(string property, bool value)
			: this(property, (object)value)
		{
		}

		/// <summary>
		/// Gets the property to filter by.
		/// </summary>
		public string Property { get; private set; }

		/// <summary>
		/// The constant value to filter by.
		/// </summary>
		public object Value { get; private set; }
	}
}
