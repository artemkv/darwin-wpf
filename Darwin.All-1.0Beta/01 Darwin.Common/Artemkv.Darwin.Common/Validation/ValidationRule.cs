using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Validation
{
	public abstract class ValidationRule : System.Attribute
	{
		public abstract bool IsValid(object propValue);
	}
}
