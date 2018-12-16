using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common.Validation
{
	[Serializable]
	public class ValidationException : Exception
	{
		public ValidationException()
			: base()
		{
		}

		public ValidationException(string message)
			: base(message)
		{
		}

		public ValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
