using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Mapping
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
	public class ObjectViewPropertyAttribute : Attribute
	{
		public ObjectViewPropertyAttribute(Type objectType)
		{
			this.ObjectType = objectType;
		}

		public Type ObjectType { get; private set; }
	}
}
