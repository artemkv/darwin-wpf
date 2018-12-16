using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Server.DataSources;
using System.Linq.Expressions;
using Artemkv.Darwin.ERModel;
using System.Reflection;

namespace Artemkv.Darwin.Server.TransactionScripts
{
	/// <summary>
	/// Retireves a collection of objects from the database and returns as a collection of DTO.
	/// </summary>
	public class GetObjectList : ITransactionScript
	{
		public ResponseBase Execute(RequestBase request)
		{
			var getObjectListRequest = request as GetObjectListRequest;
			if (getObjectListRequest == null)
				throw new InvalidOperationException("Request is null or wrong request type. Expected: GetObjectListRequest.");

			int pageSize = getObjectListRequest.PageSize;
			int pageNumber = getObjectListRequest.PageNumber;

			QueryResult<PersistentObjectDTO> queryResult = null;

			var serviceLocator = ServiceLocator.GetActive();
			var dataProvider = serviceLocator.DataProvider;
			using (var session = dataProvider.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					switch (getObjectListRequest.DataSource)
					{
						case ObjectListDataSource.Projects:
							queryResult = new ListDataSource<ProjectDTO, Project>().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.DatabaseBaseEnums:
							queryResult = new ListDataSource<BaseEnumDTO, BaseEnum>().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.DatabaseDataTypes:
							queryResult = new ListDataSource<DataTypeDTO, DataType>().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.DatabaseEntities:
							queryResult = new ListDataSource<EntityDTO, Entity>().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.EntityAttributes:
							queryResult = new ListDataSource<AttributeDTO, ERModel.Attribute>().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.DiagramEntities:
							queryResult = new ListDataSource<DiagramEntityDTO, DiagramEntity>().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.DiagramRelations:
							queryResult = new DiagramRelationsDatasource().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						case ObjectListDataSource.EntitiesNotOnDiagram:
							queryResult = new EntitiesNotOnDiagramListDataSource().GetItems(getObjectListRequest.ListFilter, session, pageSize, pageNumber);
							break;
						default:
							throw new InvalidOperationException(String.Format("Unsupported data source: {0}", getObjectListRequest.DataSource));
					}
				}
			}

			return new GetObjectListResponse(queryResult.ResultSet) { ItemsTotal = queryResult.Count };
		}
	}
}