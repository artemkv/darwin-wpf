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
	public class DiagramRelationsDatasource : ListDataSource<RelationDTO, Relation>
	{
		public override QueryResult<PersistentObjectDTO> GetItems(
			ListFilter filter,
			ISession session,
			int pageSize,
			int pageNumber)
		{
			Guid diagramID = Guid.Empty;

			// TODO: get by name
			// TODO: magic literal
			// TODO: and now it is a duplicating code
			foreach (var filterParam in filter)
			{
				if (filterParam.Property.Equals("DiagramID", StringComparison.OrdinalIgnoreCase) &&
					filterParam.Value is Guid)
				{
					diagramID = (Guid)filterParam.Value;
					break;
				}
			}

			if (diagramID == Guid.Empty)
			{
				return QueryResult<PersistentObjectDTO>.Empty;
			}

			var asm = new Assembler();

			var baseQuery = from r in session.Query<Relation>()
						join pe in session.Query<Entity>() on r.PrimaryEntity.ID equals pe.ID
						join fe in session.Query<Entity>() on r.ForeignEntity.ID equals fe.ID
						where
							(from pde in session.Query<DiagramEntity>() where pde.Entity.ID == pe.ID && pde.Diagram.ID == diagramID select pde).Any() &&
							(from fde in session.Query<DiagramEntity>() where fde.Entity.ID == fe.ID && fde.Diagram.ID == diagramID select fde).Any()
						select r;
			var pagedQuery = baseQuery.Skip(pageNumber * pageSize).Take(pageSize);

			using (var transaction = session.BeginTransaction())
			{
				var resultSet = from x in pagedQuery.ToList<Relation>() select asm.GetDTO(x);
				return new QueryResult<PersistentObjectDTO>(resultSet.ToList<PersistentObjectDTO>(), baseQuery.Count());
			}
		}
	}
}
