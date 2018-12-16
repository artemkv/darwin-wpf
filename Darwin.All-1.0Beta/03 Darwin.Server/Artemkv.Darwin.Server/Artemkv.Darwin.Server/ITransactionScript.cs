using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.Responses;
using Artemkv.Darwin.Common.Requests;

namespace Artemkv.Darwin.Server
{
	/// <summary>
	/// Represents the server operation.
	/// </summary>
	public interface ITransactionScript
	{
		/// <summary>
		/// Executes the server operation.
		/// </summary>
		/// <param name="request">The request which provides parameters required for the execution.</param>
		/// <returns>The response which contains the result of the execution.</returns>
		ResponseBase Execute(RequestBase request);
	}
}
