WITH A AS
(
	SELECT
		DENSE_RANK() OVER(
			ORDER BY 
				pk.table_schema
				, pk.table_name
				, fk.table_schema
				, fk.table_name
		) AS "TABLE_NUMBER"
		, pk.table_schema + '.' + pk.table_name AS "PrimaryTableName"
		, fk.table_schema + '.' + fk.table_name AS "ForeignTableName"
		, c.constraint_name AS "RelationName"
		, pcu.column_name AS "PrimaryColumnName"
		, fcu.column_name AS "ForeignColumnName"
		-- Is Unique?
		, CASE WHEN EXISTS 
		(
			SELECT 
				COUNT(unique_constraint_columns.column_name), 
				COUNT(current_constraint_columns.column_name) 
			FROM
			(
				-- columns-parts of any unique index in fk table
				SELECT ucu.*
				FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS uc
					LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ucu ON ucu.constraint_name = uc.constraint_name
				WHERE 
					uc.constraint_type = 'UNIQUE'
					AND
					uc.table_catalog = fk.table_catalog
					AND
					uc.table_schema = fk.table_schema
					AND
					uc.table_name = fk.table_name
			) AS unique_constraint_columns
			LEFT JOIN
			(
				-- columns in current foreign key
				SELECT ccu.*
				FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS cc
					LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ccu ON ccu.constraint_name = cc.constraint_name
				WHERE 
					cc.constraint_name = fk.constraint_name
			) AS current_constraint_columns
				ON current_constraint_columns.column_name = unique_constraint_columns.column_name
			GROUP BY unique_constraint_columns.constraint_name
			HAVING 
				-- every column from unique index has match in fk
				COUNT(unique_constraint_columns.column_name) = COUNT(current_constraint_columns.column_name)
			AND
				-- and every fk column has match in unique index
				COUNT(unique_constraint_columns.column_name) = 
					(
						-- number of columns in foreign key
						SELECT COUNT(*)
						FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS cc
							LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ccu ON ccu.constraint_name = cc.constraint_name
						WHERE 
							cc.constraint_name = fk.constraint_name
					)
		) THEN 1 ELSE 0 END AS "IsUnique"
		, pcu.ordinal_position AS "OrdinalPosition"
	FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS c
		LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk
			ON pk.constraint_schema = c.unique_constraint_schema AND pk.constraint_name = c.unique_constraint_name
		LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS fk
			ON fk.constraint_schema = c.constraint_schema AND fk.constraint_name = c.constraint_name
		LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE pcu
			ON pcu.constraint_schema = c.unique_constraint_schema AND pcu.constraint_name = c.unique_constraint_name
		LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE fcu
			ON fcu.constraint_schema = c.constraint_schema AND fcu.constraint_name = c.constraint_name AND pcu.ordinal_position = fcu.ordinal_position
)
SELECT * 
FROM A 
WHERE TABLE_NUMBER > @TABLE_NUMBER AND TABLE_NUMBER <= (@TABLE_NUMBER + @TABLES_IN_ONE_CALL)
ORDER BY
	"TABLE_NUMBER"
	, "RelationName"
	, "OrdinalPosition"