using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;

namespace Artemkv.Darwin.Services
{
	/// <summary>
	/// The service contract. 
	/// Defines all the server operations which can be accessed from the client.
	/// </summary>
	[ServiceContract]
	public interface IDarwinDataService
	{
		/// <summary>
		/// The service which is used to retrieve a single object from the server.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The response.</returns>
		[OperationContract]
		GetObjectResponse GetObject(GetObjectRequest request);

		/// <summary>
		/// The service which is used to save an object.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The response.</returns>
		[OperationContract]
		SaveObjectResponse SaveObject(SaveObjectRequest request);

		/// <summary>
		/// The service which is used to delete an object.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The response.</returns>
		[OperationContract]
		DeleteObjectResponse DeleteObject(DeleteObjectRequest request);

		/// <summary>
		/// The service which is used to retrieve a list of objects from the server.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The response.</returns>
		[OperationContract]
		GetObjectListResponse GetObjectList(GetObjectListRequest request);

		/// <summary>
		/// The service which is used to retrieve an object tree nodes from the server.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The response.</returns>
		[OperationContract]
		GetTreeNodesResponse GetTreeNodes(GetTreeNodesRequest request);
	}
}
