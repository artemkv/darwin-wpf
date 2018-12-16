using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Controller.DataSources;
using Artemkv.Darwin.Controller.Filters;

namespace Artemkv.Darwin.Controller
{
	internal class RelationInfo
	{
		public List<RelationDTO> GetDiagramRelations(Guid diagramID)
		{
			// Here might be some additional functionality, like caching.

			var filter = new DiagramRelationsFilter(diagramID);
			return new DiagramRelationsListDataSource().GetPlainDtoList(filter);
		}
	}
}
