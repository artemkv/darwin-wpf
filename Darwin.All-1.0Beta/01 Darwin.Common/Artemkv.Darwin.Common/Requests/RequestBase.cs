using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Artemkv.Darwin.Common.Requests
{
	[Serializable]
	[KnownType(typeof(GetObjectRequest))]
	[KnownType(typeof(SaveObjectRequest))]
	[KnownType(typeof(GetTreeNodesRequest))]
	[KnownType(typeof(ListFilter))]
	[KnownType(typeof(ListFilterParameter))]
	public abstract class RequestBase
	{
	}
}
