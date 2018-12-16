using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Controller
{
	internal class Paging
	{
		public const int DefaultPageSize = 20;

		public Paging(int pageSize = DefaultPageSize)
		{
			this.PageSize = pageSize;
		}

		public int PageSize { get; private set; }
	}
}
