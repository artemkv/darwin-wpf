using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Data.Mappings
{
	public class DiagramMap : ClassMap<Diagram>
	{
		public DiagramMap()
		{
			Id(x => x.ID).GeneratedBy.Assigned();
			Version(x => x.TS).Generated.Always();

			Map(x => x.DiagramName);
			Map(x => x.PaperSize);
			Map(x => x.IsVertical);
			Map(x => x.IsAdjusted);
			Map(x => x.ShowModality);

			References(x => x.Database).Column("DatabaseId"); // TODO: implement a convention to avoid specifying the foreign key name.

			HasMany(x => x.Entities)
				.KeyColumn("DiagramId")
				.Access.CamelCaseField(Prefix.Underscore)
				.Inverse();
		}
	}
}
