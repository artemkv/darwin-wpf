using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Data.Mappings
{
	public class EntityMap : ClassMap<Entity>
	{
		public EntityMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.SchemaName);
			Map(x => x.EntityName);

			References(x => x.Database).Column("DatabaseId"); // TODO: implement a convention to avoid specifying the foreign key name.

			HasMany(x => x.Attributes)
				.KeyColumn("EntityId")
				.Access.CamelCaseField(Prefix.Underscore)
				.Inverse();
		}
	}
}
