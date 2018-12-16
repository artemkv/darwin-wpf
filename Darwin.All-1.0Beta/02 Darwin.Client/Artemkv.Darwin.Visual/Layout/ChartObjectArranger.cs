using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;

namespace Artemkv.Darwin.Visual.Layout
{
	public interface IChartObjectArranger
	{
		void Arrange(List<Primitive> items, List<Connector> links);
	}
}
