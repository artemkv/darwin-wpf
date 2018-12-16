using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Artemkv.Darwin.Server;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Server.TransactionScripts;

namespace Artemkv.Darwin.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class DarwinDataService : IDarwinDataService
	{
		public GetObjectResponse GetObject(GetObjectRequest request)
		{
			var response = new GetObject().Execute(request) as GetObjectResponse;
			if (response == null)
				throw new InvalidOperationException("Response is null or wrong response type. Expected: GetObjectResponse");
			return response;
		}

		// TODO: concurrency
		public SaveObjectResponse SaveObject(SaveObjectRequest request)
		{
			var response = new SaveObject().Execute(request) as SaveObjectResponse;
			if (response == null)
				throw new InvalidOperationException("Response is null or wrong response type. Expected: SaveObjectResponse");
			return response;
		}

		public DeleteObjectResponse DeleteObject(DeleteObjectRequest request)
		{
			var response = new DeleteObject().Execute(request) as DeleteObjectResponse;
			if (response == null)
				throw new InvalidOperationException("Response is null or wrong response type. Expected: DeleteObjectResponse");
			return response;
		}

		public GetObjectListResponse GetObjectList(GetObjectListRequest request)
		{
			var response = new GetObjectList().Execute(request) as GetObjectListResponse;
			if (response == null)
				throw new InvalidOperationException("Response is null or wrong response type. Expected: GetObjectListResponse");
			return response;
		}

		public GetTreeNodesResponse GetTreeNodes(GetTreeNodesRequest request)
		{
			var response = new GetTreeNodes().Execute(request) as GetTreeNodesResponse;
			if (response == null)
				throw new InvalidOperationException("Response is null or wrong response type. Expected: GetTreeNodesResponse");
			return response;
		}
	}
}
