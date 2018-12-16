using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Requests
{
	[Serializable]
	public class GetObjectRequest : RequestBase
	{
		public GetObjectRequest(string objectType, Guid objectID)
		{
			if (String.IsNullOrWhiteSpace(objectType))
				throw new ArgumentNullException("objectType");
			if (objectID == Guid.Empty)
				throw new ArgumentNullException("objectID");

			this.ObjectType = objectType;
			this.ObjectID = objectID;
		}

		public string ObjectType { get; private set; }
		public Guid ObjectID { get; private set; }
	}
}
