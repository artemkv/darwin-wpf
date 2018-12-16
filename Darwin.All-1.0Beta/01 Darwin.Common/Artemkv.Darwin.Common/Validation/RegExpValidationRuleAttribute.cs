using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Artemkv.Darwin.ERModel;
using System.Text.RegularExpressions;

namespace Artemkv.Darwin.Common.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class RegExpValidationRuleAttribute : ValidationRule
	{
		public RegExpValidationRuleAttribute(string expression)
		{
			this.Expression = expression;
			this.ExpressionDescription = String.Empty;
		}

		public RegExpValidationRuleAttribute(string expression, string expressionDescription)
		{
			this.Expression = expression;
			this.ExpressionDescription = expressionDescription == null ? String.Empty : expressionDescription;
		}

		public string Expression { get; private set; }
		public string ExpressionDescription { get; private set; }

		public override bool IsValid(object propValue)
		{
			if (propValue == null)
			{
				return false;
			}
			if (!Regex.IsMatch(propValue.ToString(), Expression))
			{
				return false;
			}

			return true;
		}
	}
}
