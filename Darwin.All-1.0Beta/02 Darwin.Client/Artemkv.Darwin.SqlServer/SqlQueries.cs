using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Artemkv.Darwin.SqlServer
{
	internal static class SqlQueries
	{
		#region Constants

		private static readonly string GetColumnsQueryFilePath = @"Sql\GetColumsQuery.sql";
		private static readonly string GetRelationsQueryFilePath = @"Sql\GetRelationsQuery.sql";

		#endregion Constants

		#region Public Methods

		public static string GetColumnsQuery
		{
			get
			{
				if (!File.Exists(GetColumnsQueryFilePath))
				{
					throw new InvalidOperationException(String.Format("Query file '{0}' does not exist", GetColumnsQueryFilePath));
				}

				return File.ReadAllText(GetColumnsQueryFilePath);
			}
		}

		public static string GetRelationsQuery
		{
			get
			{
				if (!File.Exists(GetRelationsQueryFilePath))
				{
					throw new InvalidOperationException(String.Format("Query file '{0}' does not exist", GetRelationsQueryFilePath));
				}

				return File.ReadAllText(GetRelationsQueryFilePath);
			}
		}

		#endregion Public Methods
	}
}