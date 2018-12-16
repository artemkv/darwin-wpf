using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using FluentNHibernate.Mapping;

namespace Artemkv.Darwin.Data.Mappings
{
	public class RelationItemMap : ClassMap<RelationItem>
	{
		public RelationItemMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			References(x => x.Relation).Column("RelationId"); // TODO: implement a convention to avoid specifying the foreign key name.
			References(x => x.PrimaryAttribute).Column("PrimaryAttributeId"); // TODO: implement a convention to avoid specifying the foreign key name.
			References(x => x.ForeignAttribute).Column("ForeignAttributeId"); // TODO: implement a convention to avoid specifying the foreign key name.
		}
	}
}
