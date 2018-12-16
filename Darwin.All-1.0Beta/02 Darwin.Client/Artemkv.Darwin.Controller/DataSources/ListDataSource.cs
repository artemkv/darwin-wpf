using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.DTO;
using System.Collections.ObjectModel;

namespace Artemkv.Darwin.Controller.DataSources
{
	public abstract class ListDataSource<T> : IListDataSource
		where T : PersistentObjectDTO
	{
		public abstract string DataSourceName { get; }

		#region Public Methods

		public QueryResult<ObjectListItem> GetItems(ListFilter filter, int pageSize, int pageNumber)
		{
			var request = new GetObjectListRequest(DataSourceName)
			{
				ListFilter = filter,
				PageSize = pageSize,
				PageNumber = pageNumber
			};
			using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
			{
				var response = proxy.GetObjectList(request);
				var items = from x in response.Items select new ObjectListItem(x);
				var observable = new ObservableCollection<ObjectListItem>();
				items.ToList<ObjectListItem>().ForEach(x => observable.Add(x));

				return new QueryResult<ObjectListItem>(observable, response.ItemsTotal);
			}
		}

		public List<T> GetPlainDtoList(ListFilter filter, int pageSize = int.MaxValue, int pageNumber = 0)
		{
			var request = new GetObjectListRequest(DataSourceName)
			{
				ListFilter = filter,
				PageSize = pageSize,
				PageNumber = pageNumber
			};
			using (var proxy = new DarwinServiceReference.DarwinDataServiceClient())
			{
				var items = from x in proxy.GetObjectList(request).Items
							select x as T;
				return items.ToList<T>();
			}
		}

		#endregion Public Methods
	}
}
