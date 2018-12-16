using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Controller.DataSources;
using Artemkv.Darwin.Controller.Filters;

namespace Artemkv.Darwin.Controller
{
	internal class DataTypeInfo
	{
		public List<DataTypeDTO> GetDataTypes(Guid databaseID)
		{
			// Here might be some additional functionality, like caching.

			var filter = new DatabaseDataTypesFilter(databaseID);
			return new DatabaseDataTypesListDataSource().GetPlainDtoList(filter);
		}
	}
}
