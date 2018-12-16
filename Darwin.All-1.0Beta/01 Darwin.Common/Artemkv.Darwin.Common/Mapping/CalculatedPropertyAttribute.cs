using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Common.Mapping
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class CalculatedPropertyAttribute : System.Attribute
	{
		public CalculatedPropertyAttribute(string propertyChain)
		{
			this.PropertyChain = propertyChain;
		}

		public string PropertyChain { get; private set; }
	}
}
