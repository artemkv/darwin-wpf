using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common.Requests
{
	[Serializable]
	public class DeleteObjectRequest : RequestBase
	{
		public DeleteObjectRequest(PersistentObjectDTO obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			this.Object = obj;
		}

		public PersistentObjectDTO Object { get; private set; }
	}
}
