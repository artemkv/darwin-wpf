using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Common.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class NotNullValidationRuleAttribute : ValidationRule
	{
		public override bool IsValid(object propValue)
		{
			if (propValue == null)
			{
				return false;
			}
			if (propValue.Equals(Guid.Empty))
			{
				return false;
			}
			if (String.IsNullOrWhiteSpace(propValue.ToString()))
			{
				return false;
			}
			return true;
		}
	}
}
