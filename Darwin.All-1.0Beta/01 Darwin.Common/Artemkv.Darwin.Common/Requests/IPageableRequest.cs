using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common.Requests
{
	public interface IPageableRequest
	{
		int PageNumber { get; set; }
		int PageSize { get; set; }
	}
}
