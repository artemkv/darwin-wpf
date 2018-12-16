using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.SqlServer
{
	public class SqlServerServices
	{
		public List<EntityDTO> ImportObjects(DatabaseDTO database, ref int lastTable)
		{
			var metadataProvider = new MetadataProvider();
			try
			{
				List<EntityDTO> entities = metadataProvider.LoadEntities(database, ref lastTable);
				entities.ForEach(e => e.DatabaseID = database.ID);
				return entities;
			}
			catch(Exception ex)
			{
				throw new InvalidOperationException("Metadata provider error", ex);
			}
		}

		public List<RelationDTO> ImportRelations(DatabaseDTO database, Dictionary<string, Guid> entityIds, Dictionary<string, Guid> attributeIds, ref int lastTable)
		{
			var metadataProvider = new MetadataProvider();
			try
			{
				List<RelationDTO> relations = metadataProvider.LoadRelations(database, entityIds, attributeIds, ref lastTable);
				return relations;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Metadata provider error", ex);
			}
		}
	}
}
