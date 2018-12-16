using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common.Responses
{
	[Serializable]
	public class GetObjectResponse : ResponseBase
	{
		public GetObjectResponse(PersistentObjectDTO obj)
		{
			this.Object = obj;
		}

		public PersistentObjectDTO Object { get; private set; }
	}
}
