using System;
using System.Collections.Generic;
using Artemkv.Darwin.Common;
namespace Artemkv.Darwin.Controller
{
	public interface IListDataSource
	{
		QueryResult<ObjectListItem> GetItems(ListFilter filter, int pageSize = int.MaxValue, int pageNumber = 0);
	}
}
