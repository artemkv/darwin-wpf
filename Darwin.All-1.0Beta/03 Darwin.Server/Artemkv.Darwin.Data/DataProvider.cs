using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Linq;
using Artemkv.Darwin.Data.Mappings;
using System.Configuration;
using System.Linq.Expressions;
using System.Collections;
using Artemkv.Darwin.Common;
using System.Data.SqlClient;
using System.Data;

namespace Artemkv.Darwin.Data
{
	public class DataProvider
	{
		#region Private Members

		private readonly string _darwinConnectionString;
		private ISessionFactory _sessionFactory;

		#endregion Private Members

		#region .Ctors

		public DataProvider()
		{
			var settings = ConfigurationManager.ConnectionStrings["Darwin"];
			if (settings == null)
				throw new InvalidOperationException("Configuration string Darwin is not set");

			_darwinConnectionString = settings.ConnectionString;
			if (String.IsNullOrWhiteSpace(_darwinConnectionString))
				throw new InvalidOperationException("Configuration string Darwin is empty");

			_sessionFactory = CreateSessionFactory(_darwinConnectionString);
		}

		#endregion .Ctors

		#region Public Methods

		/// <summary>
		/// Opens and returns the new NHibernate session.
		/// </summary>
		/// <returns>The new NHibernate session.</returns>
		public ISession OpenSession()
		{
			return _sessionFactory.OpenSession();
		}

		/// <summary>
		/// Retrieves the object with the specified id from the database.
		/// </summary>
		/// <typeparam name="T">The object type.</typeparam>
		/// <param name="id">The object id.</param>
		/// <param name="session">The NHibernate session.</param>
		/// <returns>The object.</returns>
		public T GetObject<T>(Guid id, ISession session) where T : PersistentObject
		{
			if (session == null)
				throw new ArgumentNullException("session");

			using (var transaction = session.BeginTransaction())
			{
				return session.Get<T>(id);
			}
		}

		/// <summary>
		/// Retrieves the object list from the database.
		/// </summary>
		/// <typeparam name="T">The object type.</typeparam>
		/// <param name="predicate">The predicate used as a filtering criteria.</param>
		/// <param name="session">The NHibernate session.</param>
		/// <param name="items"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageNumber"></param>
		/// <returns></returns>
		public QueryResult<T> GetObjectList<T>(
			Expression<Func<T, bool>> predicate, 
			ISession session, 
			int pageSize = Int32.MaxValue, 
			int pageNumber = 0) 
			where T : PersistentObject
		{
			if (session == null)
				throw new ArgumentNullException("session");

			using (var transaction = session.BeginTransaction())
			{
				var query = session.Query<T>().Where<T>(predicate).Select<T, T>(x => x).Skip<T>(pageNumber * pageSize).Take<T>(pageSize);
				var result = new QueryResult<T>(query.ToList<T>(), session.Query<T>().Where<T>(predicate).Select<T, T>(x => x).Count<T>()); 
				// TODO: Use ToFuture, 
				// .GroupBy(x => x.ID).Select(x => x.Count()).ToFutureValue();
				// (Count() method causes LINQ to immediately execute the query even before ToFuture is applied.)
				// to avoid roundrip to db
				return result;
			}
		}

		public bool DirectDelete(Type type, Guid id, byte[] ts)
		{
			if (type == typeof(Entity))
			{
				using (var connection = new SqlConnection(_darwinConnectionString))
				{
					var command = PrepareDeleteCommand(connection, "dbo.DeleteEntity", id, ts);
					connection.Open();
					command.ExecuteNonQuery();
				}
				return true;
			}

			return false;
		}

		#endregion Public Methods

		#region Private Methods

		private ISessionFactory CreateSessionFactory(string connectionString)
		{
			return Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ProjectMap>())
				.BuildSessionFactory();
		}

		private static SqlCommand PrepareDeleteCommand(SqlConnection connection, string commandText, Guid id, byte[] ts)
		{
			var command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = commandText;

			var idParam = command.CreateParameter();
			idParam.ParameterName = "Id";
			idParam.SqlDbType = SqlDbType.UniqueIdentifier;
			idParam.SqlValue = id;
			command.Parameters.Add(idParam);

			var tsParam = command.CreateParameter();
			tsParam.ParameterName = "Ts";
			tsParam.SqlDbType = SqlDbType.Timestamp;
			tsParam.SqlValue = ts;
			command.Parameters.Add(tsParam);

			return command;
		}

		#endregion Private Methods
	}
}
