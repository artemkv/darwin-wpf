using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.ERModel;
using Artemkv.Darwin.Common.Mapping;

namespace Artemkv.Darwin.Common.DTO
{
	[Serializable]
	public class DiagramDTO : PersistentObjectDTO
	{
		private readonly List<DiagramEntityDTO> _entities = new List<DiagramEntityDTO>();

		public DiagramDTO()
		{
		}

		[ParentObject(typeof(Database))]
		public Guid DatabaseID { get; set; }

		[SimpleProperty]
		public string DiagramName { get; set; }
		[SimpleProperty]
		public int PaperSize { get; set; }
		[SimpleProperty]
		public bool IsVertical { get; set; }
		[SimpleProperty]
		public bool IsAdjusted { get; set; }
		[SimpleProperty]
		public bool ShowModality { get; set; }

		[ObjectCollection(parentProperty: "Diagram")]
		public virtual List<DiagramEntityDTO> Entities
		{
			get
			{
				return _entities;
			}
		}

		public override Type PersistentType
		{
			get { return typeof(Diagram); }
		}

		public override string ToString()
		{
			return DiagramName;
		}
	}
}
