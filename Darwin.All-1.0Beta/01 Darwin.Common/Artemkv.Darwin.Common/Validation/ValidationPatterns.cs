using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Validation
{
	public class ValidationPatterns
	{
		public const string DbIdentifierValidationPattern = @"^[A-Za-z_][A-Za-z0-9_@$#]{0,127}$";
		public const string DbIdentifierValidationPatternDescription =
			@"Value must be a string, maximum 128 characters long, which begins with a letter or an underscore (_). Subsequent characters can be letters, decimal numbers, at sign (@), dollar sign ($), number sign(#), or underscore.";
	}
}
