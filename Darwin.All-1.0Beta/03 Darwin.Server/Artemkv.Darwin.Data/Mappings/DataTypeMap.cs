using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using FluentNHibernate.Mapping;

namespace Artemkv.Darwin.Data.Mappings
{
	public class DataTypeMap : ClassMap<DataType>
	{
		public DataTypeMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.TypeName);
			Map(x => x.HasLength);

			References(x => x.BaseEnum).Column("BaseEnumId"); // TODO: implement a convention to avoid specifying the foreign key name.
			References(x => x.Database).Column("DatabaseId"); // TODO: implement a convention to avoid specifying the foreign key name.
		}
	}
}
