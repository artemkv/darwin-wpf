using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Responses
{
	[Serializable]
	public class SaveObjectResponse : ResponseBase
	{
		public string Error { get; set; }
	}
}
