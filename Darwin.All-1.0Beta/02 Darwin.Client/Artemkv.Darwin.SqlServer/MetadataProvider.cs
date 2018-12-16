using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.SqlServer
{
	/// <summary>
	/// Provides access to the Sql Server database metadata.
	/// </summary>
	internal class MetadataProvider
	{
		internal List<EntityDTO> LoadEntities(DatabaseDTO database, ref int lastTable)
		{
			const int ExpectedColumnsTotal = 9;
			const int TablesPerCall = 4;

			// TODO: merge entities, attributes, not only data types

			var dataTypes = new Dictionary<string, DataTypeDTO>();
			var entities = new List<EntityDTO>();

			foreach(var dataType in database.DataTypes)
			{
				dataTypes.Add(dataType.TypeName, dataType);
			}

			var commandText = SqlQueries.GetColumnsQuery;
			using (var connection = new SqlConnection(database.ConnectionString))
			{
				var command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				command.CommandText = commandText;

				var param1 = new SqlParameter();
				param1.ParameterName = "@TABLE_NUMBER";
				param1.SqlDbType = SqlDbType.Int;
				param1.Direction = ParameterDirection.Input;
				param1.Value = lastTable;
				command.Parameters.Add(param1);

				var param2 = new SqlParameter();
				param2.ParameterName = "@TABLES_IN_ONE_CALL";
				param2.SqlDbType = SqlDbType.Int;
				param2.Direction = ParameterDirection.Input;
				param2.Value = TablesPerCall;
				command.Parameters.Add(param2);

				connection.Open();
				var reader = command.ExecuteReader();
				{
					if (reader.HasRows)
					{
						if (reader.FieldCount != ExpectedColumnsTotal)
							throw new InvalidOperationException(String.Format("Query returned less or more than {0} columns ({1})", ExpectedColumnsTotal, commandText));

						string lastTableSchema = string.Empty;
						string lastTableName = string.Empty;
						EntityDTO entity = null;

						int tablesProcessed = 0;
						while (reader.Read())
						{
							lastTable = Convert.ToInt32(reader["TABLE_NUMBER"]);

							string tableSchema = reader["SchemaName"].ToString();
							string tableName = reader["TableName"].ToString();

							// New table
							if (!tableSchema.Equals(lastTableSchema, StringComparison.InvariantCultureIgnoreCase) ||
								!tableName.Equals(lastTableName, StringComparison.InvariantCultureIgnoreCase))
							{
								entity = new EntityDTO()
								{
									DatabaseID = database.ID,
									SchemaName = tableSchema,
									EntityName = tableName
								};
								entities.Add(entity);
								tablesProcessed++;
							}

							var attribute = new AttributeDTO()
							{
								EntityID = entity.ID,
								AttributeName = reader["ColumnName"].ToString(),
								IsRequired = Convert.ToInt32(reader["IsNullable"]) == 0 ? true : false,
								IsPrimaryKey = Convert.ToInt32(reader["IsPrimaryKey"]) == 0 ? false : true,
							};

							var attributeTypeName = reader["DataType"].ToString();
							if (!dataTypes.ContainsKey(attributeTypeName))
							{
								var dataType = new DataTypeDTO()
								{
									DatabaseID = database.ID,
									TypeName = attributeTypeName
								};
								dataTypes.Add(attributeTypeName, dataType);
								database.DataTypes.Add(dataType);
							}
							var attributeType = dataTypes[attributeTypeName];
							attribute.DataTypeID = attributeType.ID;

							entity.Attributes.Add(attribute);

							lastTableSchema = tableSchema;
							lastTableName = tableName;
						}
					}
				}
				reader.Close();
			}

			return entities;
		}

		internal List<RelationDTO> LoadRelations(DatabaseDTO database, Dictionary<string, Guid> entityIds, Dictionary<string, Guid> attributeIds, ref int lastTable)
		{
			const int ExpectedColumnsTotal = 8;
			const int TablesPerCall = 4; // TODO: move to config

			var relations = new List<RelationDTO>();

			var commandText = SqlQueries.GetRelationsQuery;
			using (var connection = new SqlConnection(database.ConnectionString))
			{
				var command = connection.CreateCommand();
				command.CommandType = CommandType.Text;
				command.CommandText = commandText;

				var param1 = new SqlParameter();
				param1.ParameterName = "@TABLE_NUMBER";
				param1.SqlDbType = SqlDbType.Int;
				param1.Direction = ParameterDirection.Input;
				param1.Value = lastTable;
				command.Parameters.Add(param1);

				var param2 = new SqlParameter();
				param2.ParameterName = "@TABLES_IN_ONE_CALL";
				param2.SqlDbType = SqlDbType.Int;
				param2.Direction = ParameterDirection.Input;
				param2.Value = TablesPerCall;
				command.Parameters.Add(param2);

				connection.Open();
				var reader = command.ExecuteReader();
				{
					if (reader.HasRows)
					{
						if (reader.FieldCount != ExpectedColumnsTotal)
							throw new InvalidOperationException(String.Format("Query returned less or more than {0} columns ({1})", ExpectedColumnsTotal, commandText));

						string lastPrimaryTableName = string.Empty;
						string lastForeignTableName = string.Empty;
						string lastRelationName = string.Empty;
						RelationDTO relation = null;

						int relationsProcessed = 0;
						while (reader.Read())
						{
							lastTable = Convert.ToInt32(reader["TABLE_NUMBER"]);

							string primaryTableName = reader["PrimaryTableName"].ToString();
							string foreignTableName = reader["ForeignTableName"].ToString();
							string relationName = reader["RelationName"].ToString();

							// New relation
							if (!primaryTableName.Equals(lastPrimaryTableName, StringComparison.InvariantCultureIgnoreCase) ||
								!foreignTableName.Equals(lastForeignTableName, StringComparison.InvariantCultureIgnoreCase) ||
								!relationName.Equals(lastRelationName, StringComparison.InvariantCultureIgnoreCase))
							{
								relation = new RelationDTO()
								{
									RelationName = relationName,
									PrimaryEntityID = entityIds[primaryTableName],
									ForeignEntityID = entityIds[foreignTableName],
									OneToOne = Convert.ToInt32(reader["IsUnique"]) == 1
								};
								relations.Add(relation);
								relationsProcessed++;
							}

							string primaryColumnName = reader["PrimaryColumnName"].ToString();
							string foreignColumnName = reader["ForeignColumnName"].ToString();

							var relationItem = new RelationItemDTO()
							{
								RelationID = relation.ID,
								PrimaryAttributeID = attributeIds[primaryTableName + "." + primaryColumnName],
								ForeignAttributeID = attributeIds[foreignTableName + "." + foreignColumnName]								
							};

							relation.Items.Add(relationItem);

							lastPrimaryTableName = primaryTableName;
							lastForeignTableName = foreignTableName;
							lastRelationName = relationName;
						}
					}
				}
				reader.Close();
			}

			return relations;
		}
	}
}
