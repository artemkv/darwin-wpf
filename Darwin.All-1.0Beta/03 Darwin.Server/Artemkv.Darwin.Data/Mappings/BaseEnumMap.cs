using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using FluentNHibernate.Mapping;

namespace Artemkv.Darwin.Data.Mappings
{
	public class BaseEnumMap : ClassMap<BaseEnum>
	{
		public BaseEnumMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.BaseEnumName);

			References(x => x.Database).Column("DatabaseId"); // TODO: implement a convention to avoid specifying the foreign key name.

			HasMany(x => x.Values)
				.KeyColumn("BaseEnumId")
				.Access.CamelCaseField(Prefix.Underscore)
				.Inverse();
		}
	}
}
