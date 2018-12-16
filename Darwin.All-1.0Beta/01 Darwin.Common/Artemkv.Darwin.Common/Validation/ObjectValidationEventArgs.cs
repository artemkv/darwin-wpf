using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Validation
{
	public class ObjectValidationEventArgs : EventArgs
	{
		public ObjectValidationEventArgs(bool isValid, ObjectPropertyValidationDetails details)
		{
			if (details == null)
				throw new ArgumentNullException("details");

			this.IsValid = isValid;
			this.Details = details;
		}

		public bool IsValid { get; private set; }
		public ObjectPropertyValidationDetails Details { get; private set; }
	}
}
