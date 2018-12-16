using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Mapping
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
	public class ObjectPropertyAttribute : Attribute
	{
		public ObjectPropertyAttribute(Type objectType)
		{
			this.ObjectType = objectType;
		}

		public Type ObjectType { get; private set; }
	}
}
