using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using FluentNHibernate.Mapping;

namespace Artemkv.Darwin.Data.Mappings
{
	public class AttributeMap : ClassMap<ERModel.Attribute>
	{
		public AttributeMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.AttributeName);
			Map(x => x.Length).Column("AttributeLength");
			Map(x => x.IsRequired);
			Map(x => x.IsPrimaryKey);

			References(x => x.DataType).Column("DataTypeId"); // TODO: implement a convention to avoid specifying the foreign key name.
			References(x => x.Entity).Column("EntityId"); // TODO: implement a convention to avoid specifying the foreign key name.
		}
	}
}
