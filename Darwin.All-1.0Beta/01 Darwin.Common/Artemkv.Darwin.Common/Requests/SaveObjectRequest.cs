using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common.Requests
{
	[Serializable]
	public class SaveObjectRequest : RequestBase
	{
		public SaveObjectRequest(PersistentObjectDTO obj, bool bypassValidation = false)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			this.Object = obj;
			this.BypassValidation = bypassValidation;
		}

		public PersistentObjectDTO Object { get; private set; }
		public bool BypassValidation { get; private set; }
	}
}
