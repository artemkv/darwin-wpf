using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Data.Mappings
{
	public class RelationMap : ClassMap<Relation>
	{
		public RelationMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.RelationName);
			Map(x => x.OneToOne);
			Map(x => x.AtLeastOne);

			References(x => x.PrimaryEntity).Column("PrimaryEntityId"); // TODO: implement a convention to avoid specifying the foreign key name.
			References(x => x.ForeignEntity).Column("ForeignEntityId"); // TODO: implement a convention to avoid specifying the foreign key name.

			HasMany(x => x.Items)
				.KeyColumn("RelationId")
				.Access.CamelCaseField(Prefix.Underscore)
				.Inverse();
		}
	}
}
