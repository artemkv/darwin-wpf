using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Artemkv.Darwin.Common.Validation;

namespace Artemkv.Darwin.Common.Responses
{
	[Serializable]
	[KnownType(typeof(GetObjectResponse))]
	[KnownType(typeof(SaveObjectResponse))]
	[KnownType(typeof(GetTreeNodesResponse))]
	public abstract class ResponseBase
	{
	}
}
