using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Controller.Filters;
using Artemkv.Darwin.Controller.DataSources;

namespace Artemkv.Darwin.Controller
{
	internal class DiagramEntityInfo
	{
		public List<DiagramEntityDTO> GetDiagramEntities(Guid diagramID)
		{
			// Here might be some additional functionality, like caching.

			var filter = new DiagramEntitiesFilter(diagramID);
			return new DiagramEntitiesListDataSource().GetPlainDtoList(filter);
		}
	}
}
