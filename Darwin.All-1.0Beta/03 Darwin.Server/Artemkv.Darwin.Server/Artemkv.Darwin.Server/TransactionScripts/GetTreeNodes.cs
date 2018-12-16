using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Server.DataSources;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;

namespace Artemkv.Darwin.Server.TransactionScripts
{
	/// <summary>
	/// Returns the collection of tree nodes.
	/// </summary>
	public class GetTreeNodes : ITransactionScript
	{
		public ResponseBase Execute(RequestBase request)
		{
			var getNodesRequest = request as GetTreeNodesRequest;
			if (getNodesRequest == null)
				throw new InvalidOperationException("Request is null or wrong request type. Expected: GetTreeNodesRequest.");
			IList<TreeNode> nodes = null;

			var serviceLocator = ServiceLocator.GetActive();
			var dataProvider = serviceLocator.DataProvider;
			using (var session = dataProvider.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					switch (getNodesRequest.DataSource)
					{
						case ObjectTreeDataSource.ProjectTreeView:
							nodes = new ProjectTreeViewDataSource().GetNodes(
								getNodesRequest.ParentID, 
								getNodesRequest.Path, 
								session, 
								getNodesRequest.PageSize, 
								getNodesRequest.PageNumber);
							break;
						default:
							throw new InvalidOperationException(String.Format("Unsupported data source: {0}", getNodesRequest.DataSource));
					}
				}
			}

			return new GetTreeNodesResponse(nodes);
		}
	}
}