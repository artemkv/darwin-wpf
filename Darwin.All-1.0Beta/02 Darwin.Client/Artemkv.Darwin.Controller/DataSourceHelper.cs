using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.Requests;

namespace Artemkv.Darwin.Controller
{
	public class DataSourceHelper
	{
		public static IEnumerable<T> CombinePages<T>(Func<IPageableRequest, IEnumerable<T>> func, IPageableRequest request)
		{
			var serviceLocator = ServiceLocator.GetActive();

			int pageSize = serviceLocator.Paging.PageSize;
			request.PageSize = pageSize;

			var allItems = new List<T>();
			int rowsRetrieved = 0;
			int pageNumber = 0;
			do
			{
				request.PageNumber = pageNumber;
				var items = func(request);
				allItems.AddRange(items);
				rowsRetrieved = items.Count();
				pageNumber++;
			} while (rowsRetrieved == pageSize);
			return allItems;
		}
	}
}
