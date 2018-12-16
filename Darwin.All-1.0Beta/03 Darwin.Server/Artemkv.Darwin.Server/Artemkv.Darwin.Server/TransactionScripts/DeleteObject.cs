using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using NHibernate;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;
using NHibernate.Exceptions;
using System.Data.SqlClient;

namespace Artemkv.Darwin.Server.TransactionScripts
{
	/// <summary>
	/// Deletes an object.
	/// </summary>
	public class DeleteObject : ITransactionScript
	{
		public ResponseBase Execute(RequestBase request)
		{
			var response = new DeleteObjectResponse();
			try
			{
				var deleteObjectRequest = request as DeleteObjectRequest;
				if (deleteObjectRequest == null)
					throw new InvalidOperationException("Request is null or wrong request type. Expected: DeleteObjectRequest.");

				var serviceLocator = ServiceLocator.GetActive();
				var dataProvider = serviceLocator.DataProvider;

				// Custom delete
				if (!dataProvider.DirectDelete(deleteObjectRequest.Object.PersistentType, deleteObjectRequest.Object.ID, deleteObjectRequest.Object.TS))
				{
					using (ISession session = dataProvider.OpenSession())
					{
						using (var transaction = session.BeginTransaction())
						{
							try
							{
								var realObject = new Assembler().GetRealObject(deleteObjectRequest.Object, session);
								session.Delete(realObject);
								transaction.Commit();
							}
							catch
							{
								transaction.Rollback();
								throw;
							}
						}
					}
				}

				return response;
			}
			catch (GenericADOException adoException)
			{
				var inner = adoException.InnerException;
				if (inner == null)
				{
					throw;
				}
				var sqlException = inner as SqlException;
				if (sqlException != null)
				{
					response.Error = sqlException.Message;
					return response;
				}
				throw;
			}
		}
	}
}
