using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.ERModel;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Data;

namespace Artemkv.Darwin.Server.DataSources
{
	public class EntitiesNotOnDiagramListDataSource : ListDataSource<EntityDTO, Entity>
	{
		public override QueryResult<PersistentObjectDTO> GetItems(
			ListFilter filter,
			ISession session,
			int pageSize,
			int pageNumber)
		{
			Guid diagramID = Guid.Empty;
			Guid databaseID = Guid.Empty;

			// TODO: get by name
			// TODO: magic literal
			foreach (var filterParam in filter)
			{
				if (filterParam.Property.Equals("Diagram.ID", StringComparison.OrdinalIgnoreCase) &&
					filterParam.Value is Guid)
				{
					diagramID = (Guid)filterParam.Value;
				}
				if (filterParam.Property.Equals("Database.ID", StringComparison.OrdinalIgnoreCase) &&
					filterParam.Value is Guid)
				{
					databaseID = (Guid)filterParam.Value;
				}
			}

			if (diagramID == Guid.Empty)
			{
				return QueryResult<PersistentObjectDTO>.Empty;
			}
			if (databaseID == Guid.Empty)
			{
				return QueryResult<PersistentObjectDTO>.Empty;
			}

			var asm = new Assembler();
			var baseQuery = from e1 in
							session.Query<Entity>().Where<Entity>(x =>
								!(from e2 in session.Query<Entity>()
								 join de in session.Query<DiagramEntity>() on e2.ID equals de.Entity.ID
								 where de.Diagram.ID == diagramID
								 select e2).Any<Entity>(y => x.ID == y.ID) && x.Database.ID == databaseID)
						select e1;
			var pagedQuery = baseQuery.Skip(pageNumber * pageSize).Take(pageSize);

			using (var transaction = session.BeginTransaction())
			{
				var resultSet = from x in pagedQuery.ToList<Entity>() select asm.GetDTO(x);
				return new QueryResult<PersistentObjectDTO>(resultSet.ToList<PersistentObjectDTO>(), baseQuery.Count());
			}
		}
	}
}
