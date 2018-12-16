﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Controller.DataSources
{
	public class EntitiesNotOnDiagramListDataSource : ListDataSource<DiagramEntityDTO>
	{
		public override string DataSourceName
		{
			get { return ObjectListDataSource.EntitiesNotOnDiagram; }
		}
	}
}
