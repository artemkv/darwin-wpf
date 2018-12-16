using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.Mapping;

namespace Artemkv.Darwin.Common.DTO
{
	[Serializable]
	public class ProjectDTO : PersistentObjectDTO
	{
		public ProjectDTO()
		{
		}

		[SimpleProperty]
		public string ProjectName { get; set; }

		public override Type PersistentType
		{
			get { return typeof(Project); }
		}

		public override string ToString()
		{
			return ProjectName;
		}
	}
}
