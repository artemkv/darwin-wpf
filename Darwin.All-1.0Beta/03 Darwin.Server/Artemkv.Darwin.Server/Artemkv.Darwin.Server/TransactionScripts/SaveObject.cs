using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using NHibernate;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common.Validation;

namespace Artemkv.Darwin.Server.TransactionScripts
{
	/// <summary>
	/// Builds real objects from a DTO and saves in the database.
	/// </summary>
	public class SaveObject : ITransactionScript
	{
		public ResponseBase Execute(RequestBase request)
		{
			var response = new SaveObjectResponse();
			try
			{
				var saveObjectRequest = request as SaveObjectRequest;
				if (saveObjectRequest == null)
					throw new InvalidOperationException("Request is null or wrong request type. Expected: SaveObjectRequest.");

				var serviceLocator = ServiceLocator.GetActive();
				var dataProvider = serviceLocator.DataProvider;
				using (ISession session = dataProvider.OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{
						try
						{
							var realObject = new Assembler(saveObjectRequest.BypassValidation).GetRealObject(saveObjectRequest.Object, session);
							session.Merge(realObject);
							session.Flush();
							transaction.Commit();
						}
						catch
						{
							transaction.Rollback();
							throw;
						}
					}
				}

				return response;
			}
			catch (ValidationException validationException)
			{
				response.Error = validationException.Message;
				return response;
			}
			catch (StaleObjectStateException)
			{
				response.Error = "Object has been changed in the another session.";
				return response;
			}
		}
	}
}
