using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Artemkv.Darwin.Data;

namespace Artemkv.Darwin.Server
{
	/// <summary>
	/// Services and objects locator.
	/// See the pattern description at: <see cref="http://artemkondratyev.net/createstoreget.htm"/>.
	/// </summary>
	public class ServiceLocator // TODO: should be thread-safe
	{
		#region Class Members

		/// <summary>
		/// Registry to store objects.
		/// All objects that are stored in the registry are considered to be valid 
		/// and the same all time during the session. 
		/// </summary>
		private Registry _registry = null;

		/// <summary>
		/// The single instance.
		/// </summary>
		private static ServiceLocator _self = null;

		#endregion Class Members

		#region Constructors

		/// <summary>
		/// Initializes a new instance of ServiceLocator class.
		/// </summary>
		private ServiceLocator()
		{
			_registry = new Registry();
		}

		/// <summary>
		/// Performs static initialization of ServiceLocator class.
		/// </summary>
		static ServiceLocator()
		{
			_self = new ServiceLocator();
		}

		#endregion Constructors

		#region Static Methods

		/// <summary>
		/// Returns the active instance of ServiceLocator object.
		/// <example>
		/// This code is recommended to get the active instance:
		/// <c> ServiceLocator serviceLocator = ServiceLocator.GetActive(); </c>
		/// </example>
		/// </summary>
		/// <returns>The active instance of ServiceLocator object.</returns>
		public static ServiceLocator GetActive()
		{
			return _self;
		}

		#endregion Static Methods

		#region Internal Properties

		internal DataProvider DataProvider
		{
			get
			{
				if (_registry.DataProvider == null)
				{
					_registry.DataProvider = new DataProvider();
				}

				return _registry.DataProvider;
			}
		}

		#endregion Internal Properties
	}
}
