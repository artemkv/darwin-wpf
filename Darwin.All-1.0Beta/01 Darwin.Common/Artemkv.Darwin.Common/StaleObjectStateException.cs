using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common
{
	[Serializable]
	public class StaleObjectStateException : Exception
	{
		public StaleObjectStateException() : base ()
		{
		}

		public StaleObjectStateException(string message) : base (message)
		{
		}

		public StaleObjectStateException(string message, Exception innerException)
			: base (message, innerException)
		{
		}
	}
}
