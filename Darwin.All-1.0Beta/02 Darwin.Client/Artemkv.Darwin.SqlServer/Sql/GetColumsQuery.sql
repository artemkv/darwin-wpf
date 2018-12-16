WITH A AS 
(
	SELECT
		DENSE_RANK() OVER(ORDER BY s.name, t.name) "TABLE_NUMBER"
		, s.name as "SchemaName"
		, t.name as "TableName"
		, c.name as "ColumnName"
		, c.column_id as "ColumnId"
		, st.name as "DataType"
		, c.max_length as "MaxLength"
		, c.is_nullable as "IsNullable"
		, CASE WHEN NOT kc.[object_id] IS NULL THEN 1 ELSE 0 END as "IsPrimaryKey"
	FROM sys.tables t
		JOIN sys.schemas s ON s.[schema_id] = t.[schema_id]
		JOIN sys.columns c ON c.[object_id] = t.[object_id]
		JOIN sys.types st ON c.[user_type_id] = st.[user_type_id]
		LEFT JOIN
			(
				SELECT 
					ic.[object_id]
					, ic.[column_id]
					, k.[parent_object_id]
				FROM sys.index_columns as ic
					JOIN sys.key_constraints k ON ic.[index_id] = k.[unique_index_id]
				WHERE
					k.[type] = 'PK'
			) kc
			ON
				kc.[object_id] = t.[object_id]
				AND
				kc.[column_id] = c.[column_id]
				AND 
				kc.[parent_object_id] = t.[object_id]
)
SELECT * 
FROM A 
WHERE TABLE_NUMBER > @TABLE_NUMBER AND TABLE_NUMBER <= (@TABLE_NUMBER + @TABLES_IN_ONE_CALL)
ORDER BY "TABLE_NUMBER", "ColumnId";