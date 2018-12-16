using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Visual.Layout
{
	public class SimpleArranger : IChartObjectArranger
	{
		public void Arrange(List<Primitive> items, List<Connector> links)
		{
			int columsTotal = (int)Math.Sqrt(items.Count);
			int columnCurrent = 0;

			double x = 50.00;
			double y = 50.00;
			double maxHeight = 0.00;

			foreach (Primitive item in items)
			{
				item.X = x;
				item.Y = y;

				var diagramEntity = item.Object as DiagramEntityDTO;
				if (diagramEntity != null)
				{
					diagramEntity.X = (int)x;
					diagramEntity.Y = (int)y;
				}

				x += item.Width * 2.00;
				if (maxHeight < item.Height)
				{
					maxHeight = item.Height;
				}
				columnCurrent++;

				if (columnCurrent == columsTotal)
				{
					columnCurrent = 0;
					x = 50.00;
					y += maxHeight * 2.00;
					maxHeight = 0.00;
				}
			}
		}
	}
}
