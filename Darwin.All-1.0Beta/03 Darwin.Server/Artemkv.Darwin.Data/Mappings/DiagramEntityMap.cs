using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Data.Mappings
{
	public class DiagramEntityMap : ClassMap<DiagramEntity>
	{
		public DiagramEntityMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.X);
			Map(x => x.Y);

			References(x => x.Diagram).Column("DiagramId"); // TODO: implement a convention to avoid specifying the foreign key name.
			References(x => x.Entity).Column("EntityId"); // TODO: implement a convention to avoid specifying the foreign key name.
		}
	}
}
