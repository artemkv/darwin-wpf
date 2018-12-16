using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Data;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Server.TransactionScripts
{
	/// <summary>
	/// Retireves the single object from the database and returns as a DTO.
	/// </summary>
	public class GetObject : ITransactionScript
	{
		public ResponseBase Execute(RequestBase request)
		{
			var getObjectRequest = request as GetObjectRequest;
			if (getObjectRequest == null)
				throw new InvalidOperationException("Request is null or wrong request type. Expected: GetObjectRequest.");

			PersistentObjectDTO obj = null;
			var serviceLocator = ServiceLocator.GetActive();
			var dataProvider = serviceLocator.DataProvider;
			using (var session = dataProvider.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					switch (getObjectRequest.ObjectType)
					{
						case ObjectType.Project:
							obj = new Assembler().GetDTO(dataProvider.GetObject<Project>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.Database:
							obj = new Assembler().GetDTO(dataProvider.GetObject<Database>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.DataType:
							obj = new Assembler().GetDTO(dataProvider.GetObject<DataType>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.BaseEnum:
							obj = new Assembler().GetDTO(dataProvider.GetObject<BaseEnum>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.BaseEnumValue:
							obj = new Assembler().GetDTO(dataProvider.GetObject<BaseEnumValue>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.Entity:
							obj = new Assembler().GetDTO(dataProvider.GetObject<Entity>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.Attribute:
							obj = new Assembler().GetDTO(dataProvider.GetObject<ERModel.Attribute>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.Relation:
							obj = new Assembler().GetDTO(dataProvider.GetObject<Relation>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.RelationItem:
							obj = new Assembler().GetDTO(dataProvider.GetObject<RelationItem>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.Diagram:
							obj = new Assembler().GetDTO(dataProvider.GetObject<Diagram>(getObjectRequest.ObjectID, session));
							break;
						case ObjectType.DiagramEntity:
							obj = new Assembler().GetDTO(dataProvider.GetObject<DiagramEntity>(getObjectRequest.ObjectID, session));
							break;
						default:
							throw new InvalidOperationException(String.Format("Unsupported object type: {0}", getObjectRequest.ObjectID));
					}
				}
			}
			return new GetObjectResponse(obj);
		}
	}
}
