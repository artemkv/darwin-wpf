using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Data.Mappings
{
	public class DatabaseMap : ClassMap<Database>
	{
		public DatabaseMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.DBName);
			Map(x => x.ConnectionString);

			References(x => x.Project).Column("ProjectId"); // TODO: implement a convention to avoid specifying the foreign key name.

			HasMany(x => x.DataTypes)
				.KeyColumn("DatabaseId")
				.Access.CamelCaseField(Prefix.Underscore)
				.Inverse();
		}
	}
}
