-- Drop FK
IF OBJECT_ID (N'dbo.Database', N'U') IS NOT NULL
	ALTER TABLE dbo.[Database] DROP CONSTRAINT Database_Project_FK;
GO
IF OBJECT_ID (N'dbo.BaseEnum', N'U') IS NOT NULL
	ALTER TABLE dbo.BaseEnum DROP CONSTRAINT BaseEnum_Database_FK;
GO
IF OBJECT_ID (N'dbo.BaseEnumValue', N'U') IS NOT NULL
	ALTER TABLE dbo.BaseEnumValue DROP CONSTRAINT BaseEnumValue_BaseEnum_FK;
GO
IF OBJECT_ID (N'dbo.DataType', N'U') IS NOT NULL
	ALTER TABLE dbo.DataType DROP CONSTRAINT DataType_Database_FK;
GO
IF OBJECT_ID (N'dbo.Entity', N'U') IS NOT NULL
	ALTER TABLE dbo.Entity DROP CONSTRAINT Entity_Database_FK;
GO
IF OBJECT_ID (N'dbo.Attribute', N'U') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Attribute DROP CONSTRAINT Attribute_Entity_FK;
	ALTER TABLE dbo.Attribute DROP CONSTRAINT Attribute_DataType_FK;
END
GO
IF OBJECT_ID (N'dbo.Relation', N'U') IS NOT NULL
BEGIN
	ALTER TABLE dbo.Relation DROP CONSTRAINT Relation_PkEntity_FK;
	ALTER TABLE dbo.Relation DROP CONSTRAINT Relation_FkEntity_FK;
END
GO
IF OBJECT_ID (N'dbo.RelationItem', N'U') IS NOT NULL
BEGIN
	ALTER TABLE dbo.RelationItem DROP CONSTRAINT RelationItem_Relation_FK;
	ALTER TABLE dbo.RelationItem DROP CONSTRAINT RelationItem_PkAttribute_FK;
	ALTER TABLE dbo.RelationItem DROP CONSTRAINT RelationItem_FkAttribute_FK;
END
GO
IF OBJECT_ID (N'dbo.Diagram', N'U') IS NOT NULL
	ALTER TABLE dbo.Diagram DROP CONSTRAINT Diagram_Database_FK;
GO
IF OBJECT_ID (N'dbo.DiagramEntity', N'U') IS NOT NULL
BEGIN
	ALTER TABLE dbo.DiagramEntity DROP CONSTRAINT DiagramEntity_Diagram_FK;
	ALTER TABLE dbo.DiagramEntity DROP CONSTRAINT DiagramEntity_Entity_FK;
END
GO


-- Drop tables
IF OBJECT_ID (N'dbo.Project', N'U') IS NOT NULL
	DROP TABLE dbo.Project;
GO
IF OBJECT_ID (N'dbo.Database', N'U') IS NOT NULL
	DROP TABLE dbo.[Database];
GO
IF OBJECT_ID (N'dbo.BaseEnum', N'U') IS NOT NULL
	DROP TABLE dbo.BaseEnum;
GO
IF OBJECT_ID (N'dbo.BaseEnumValue', N'U') IS NOT NULL
	DROP TABLE dbo.BaseEnumValue;
GO
IF OBJECT_ID (N'dbo.DataType', N'U') IS NOT NULL
	DROP TABLE dbo.DataType;
GO
IF OBJECT_ID (N'dbo.Entity', N'U') IS NOT NULL
	DROP TABLE dbo.Entity;
GO
IF OBJECT_ID (N'dbo.Attribute', N'U') IS NOT NULL
	DROP TABLE dbo.Attribute;
GO
IF OBJECT_ID (N'dbo.Relation', N'U') IS NOT NULL
	DROP TABLE dbo.Relation;
GO
IF OBJECT_ID (N'dbo.RelationItem', N'U') IS NOT NULL
	DROP TABLE dbo.RelationItem;
GO
IF OBJECT_ID (N'dbo.Diagram', N'U') IS NOT NULL
	DROP TABLE dbo.Diagram;
GO
IF OBJECT_ID (N'dbo.DiagramEntity', N'U') IS NOT NULL
	DROP TABLE dbo.DiagramEntity;
GO


-- Common type all for variable-length string identifiers.
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'TString' AND ss.name = N'dbo')
	DROP TYPE dbo.TString;
GO

CREATE TYPE dbo.TString FROM NVARCHAR(255);
GO


-- Project
CREATE TABLE dbo.Project
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, ProjectName TString NOT NULL
);
GO
ALTER TABLE dbo.Project ADD CONSTRAINT Project_PK PRIMARY KEY NONCLUSTERED (Id);
GO


-- Database
CREATE TABLE dbo.[Database]
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, DBName TString NOT NULL
	, ConnectionString VARCHAR(4000) NULL
	, ProjectId UNIQUEIDENTIFIER NOT NULL
);
GO
ALTER TABLE dbo.[Database] ADD CONSTRAINT Database_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.[Database] ADD CONSTRAINT Database_Project_FK FOREIGN KEY (ProjectId) REFERENCES dbo.Project(Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX Database_ProjectId_IDX ON dbo.[Database] (ProjectId);
GO


-- Base Enum
CREATE TABLE dbo.BaseEnum
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, BaseEnumName TString NOT NULL
	, DatabaseId UNIQUEIDENTIFIER NOT NULL
);
GO
ALTER TABLE dbo.BaseEnum ADD CONSTRAINT BaseEnum_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.BaseEnum ADD CONSTRAINT BaseEnum_Database_FK FOREIGN KEY (DatabaseId) REFERENCES dbo.[Database](Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX BaseEnum_DatabaseId_IDX ON dbo.BaseEnum (DatabaseId);
GO


-- Base Enum Value
CREATE TABLE dbo.BaseEnumValue
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, Name TString NOT NULL
	, Value INTEGER NOT NULL
	, BaseEnumId UNIQUEIDENTIFIER NOT NULL
);
GO
ALTER TABLE dbo.BaseEnumValue ADD CONSTRAINT BaseEnumValue_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.BaseEnumValue ADD CONSTRAINT BaseEnumValue_BaseEnum_FK FOREIGN KEY (BaseEnumId) REFERENCES dbo.BaseEnum(Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX BaseEnumValue_BaseEnumId_IDX ON dbo.BaseEnumValue (BaseEnumId);
GO
CREATE UNIQUE NONCLUSTERED INDEX BaseEnumValue_BaseEnumId_Name_UNQ ON dbo.BaseEnumValue (BaseEnumId, Name)
GO
CREATE UNIQUE NONCLUSTERED INDEX BaseEnumValue_BaseEnumId_Value_UNQ ON dbo.BaseEnumValue (BaseEnumId, Value)
GO

-- DataType
CREATE TABLE dbo.DataType
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, TypeName TString NOT NULL
	, DatabaseId UNIQUEIDENTIFIER NOT NULL
	, HasLength INT NULL CHECK (HasLength = 0 OR HasLength = 1)
	, BaseEnumId UNIQUEIDENTIFIER NULL
);
GO
ALTER TABLE dbo.DataType ADD CONSTRAINT DataType_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.DataType ADD CONSTRAINT DataType_Database_FK FOREIGN KEY (DatabaseId) REFERENCES dbo.[Database](Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX DataType_DatabaseId_IDX ON dbo.DataType (DatabaseId);
GO
ALTER TABLE dbo.DataType ADD CONSTRAINT DataType_BaseEnum_FK FOREIGN KEY (BaseEnumId) REFERENCES dbo.[BaseEnum](Id);
GO
CREATE NONCLUSTERED INDEX DataType_BaseEnumId_IDX ON dbo.DataType (BaseEnumId);
GO


-- Entity
CREATE TABLE dbo.Entity
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, SchemaName TString NOT NULL
	, EntityName TString NOT NULL
	, DatabaseId UNIQUEIDENTIFIER NOT NULL
);
GO
ALTER TABLE dbo.Entity ADD CONSTRAINT Entity_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.Entity ADD CONSTRAINT Entity_Database_FK FOREIGN KEY (DatabaseId) REFERENCES dbo.[Database](Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX Entity_DatabaseId_IDX ON dbo.Entity (DatabaseId);
GO


-- Attribute
CREATE TABLE dbo.Attribute
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, AttributeName TString NOT NULL
	, DataTypeId UNIQUEIDENTIFIER NOT NULL
	, IsRequired INT NOT NULL CHECK (IsRequired = 0 OR IsRequired = 1)
	, EntityId UNIQUEIDENTIFIER NOT NULL
	, AttributeLength INT NULL
	, IsPrimaryKey INT NULL CHECK (IsPrimaryKey = 0 OR IsPrimaryKey = 1)
);
GO
ALTER TABLE dbo.Attribute ADD CONSTRAINT Attribute_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.Attribute ADD CONSTRAINT Attribute_Entity_FK FOREIGN KEY (EntityId) REFERENCES dbo.Entity(Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX Attribute_EntityId_IDX ON dbo.Attribute (EntityId);
GO
ALTER TABLE dbo.Attribute ADD CONSTRAINT Attribute_DataType_FK FOREIGN KEY (DataTypeId) REFERENCES dbo.DataType(Id);
GO
CREATE NONCLUSTERED INDEX Attribute_DataTypeId_IDX ON dbo.Attribute (DataTypeId);
GO


-- Relation
CREATE TABLE dbo.Relation
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, RelationName TString NOT NULL
	, PrimaryEntityId UNIQUEIDENTIFIER NOT NULL
	, ForeignEntityId UNIQUEIDENTIFIER NOT NULL
	, OneToOne INT NULL CHECK (OneToOne = 0 OR OneToOne = 1)
	, AtLeastOne INT NULL CHECK (AtLeastOne = 0 OR AtLeastOne = 1)
);
GO
ALTER TABLE dbo.Relation ADD CONSTRAINT Relation_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.Relation ADD CONSTRAINT Relation_PkEntity_FK FOREIGN KEY (PrimaryEntityId) REFERENCES dbo.Entity(Id);
GO
ALTER TABLE dbo.Relation ADD CONSTRAINT Relation_FkEntity_FK FOREIGN KEY (ForeignEntityId) REFERENCES dbo.Entity(Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX Relation_PrimaryEntityId_IDX ON dbo.Relation (PrimaryEntityId);
GO
CREATE NONCLUSTERED INDEX Relation_ForeignEntityId_IDX ON dbo.Relation (ForeignEntityId);
GO


-- Relation item
CREATE TABLE dbo.RelationItem
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, RelationId UNIQUEIDENTIFIER NOT NULL
	, PrimaryAttributeId UNIQUEIDENTIFIER NOT NULL
	, ForeignAttributeId UNIQUEIDENTIFIER NOT NULL
);
GO
ALTER TABLE dbo.RelationItem ADD CONSTRAINT RelationItem_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.RelationItem ADD CONSTRAINT RelationItem_Relation_FK FOREIGN KEY (RelationId) REFERENCES dbo.Relation(Id) ON DELETE CASCADE;;
GO
ALTER TABLE dbo.RelationItem ADD CONSTRAINT RelationItem_PkAttribute_FK FOREIGN KEY (PrimaryAttributeId) REFERENCES dbo.Attribute(Id);
GO
ALTER TABLE dbo.RelationItem ADD CONSTRAINT RelationItem_FkAttribute_FK FOREIGN KEY (ForeignAttributeId) REFERENCES dbo.Attribute(Id);
GO
CREATE NONCLUSTERED INDEX RelationItem_RelationId_IDX ON dbo.RelationItem (RelationId);
GO
CREATE NONCLUSTERED INDEX RelationItem_PrimaryAttributeId_IDX ON dbo.RelationItem (PrimaryAttributeId);
GO
CREATE NONCLUSTERED INDEX RelationItem_ForeignAttributeId_IDX ON dbo.RelationItem (ForeignAttributeId);
GO


-- Diagram
CREATE TABLE dbo.Diagram
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, DiagramName TString NOT NULL
	, DatabaseId UNIQUEIDENTIFIER NOT NULL
	, PaperSize INT NULL CHECK (PaperSize >= 1 AND PaperSize <= 8)
	, IsVertical INT NULL CHECK (IsVertical = 0 OR IsVertical = 1)
	, IsAdjusted INT NULL CHECK (IsAdjusted = 0 OR IsAdjusted = 1)
	, ShowModality INT NULL CHECK (ShowModality = 0 OR ShowModality = 1)
);
GO
ALTER TABLE dbo.Diagram ADD CONSTRAINT Diagram_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.Diagram ADD CONSTRAINT Diagram_Database_FK FOREIGN KEY (DatabaseId) REFERENCES dbo.[Database](Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX Diagram_DatabaseId_IDX ON dbo.Diagram (DatabaseId);
GO


-- Diagram Entity
CREATE TABLE dbo.DiagramEntity
(
	Id UNIQUEIDENTIFIER NOT NULL
	, Ts TIMESTAMP NOT NULL
	, DiagramId UNIQUEIDENTIFIER NOT NULL
	, EntityId UNIQUEIDENTIFIER NOT NULL
	, X INTEGER NULL
	, Y INTEGER NULL
);
GO
ALTER TABLE dbo.DiagramEntity ADD CONSTRAINT DiagramEntity_PK PRIMARY KEY NONCLUSTERED (Id);
GO
ALTER TABLE dbo.DiagramEntity ADD CONSTRAINT DiagramEntity_Diagram_FK FOREIGN KEY (DiagramId) REFERENCES dbo.Diagram(Id) ON DELETE CASCADE;
GO
CREATE NONCLUSTERED INDEX DiagramEntity_DiagramId_IDX ON dbo.DiagramEntity (DiagramId);
GO
ALTER TABLE dbo.DiagramEntity ADD CONSTRAINT DiagramEntity_Entity_FK FOREIGN KEY (EntityId) REFERENCES dbo.Entity(Id);
GO
CREATE NONCLUSTERED INDEX DiagramEntity_EntityId_IDX ON dbo.DiagramEntity (EntityId);
GO