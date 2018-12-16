using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Common
{
	[Serializable]
	[KnownType(typeof(ProjectDTO))]
	[KnownType(typeof(DatabaseDTO))]
	[KnownType(typeof(EntityDTO))]
	[KnownType(typeof(DataTypeDTO))]
	[KnownType(typeof(AttributeDTO))]
	[KnownType(typeof(ObjectGroup))]
	public class TreeNode
	{
		public TreeNode(object obj, Guid objectID, string subPath, bool isLeaf = false)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (subPath == null)
				throw new ArgumentNullException("subPath");

			this.Object = obj;
			this.ObjectID = objectID;
			this.SubPath = subPath;
			this.IsLeaf = isLeaf;
		}

		public object Object { get; private set; }
		public Guid ObjectID { get; private set; }
		public string SubPath { get; private set; }
		public bool IsLeaf { get; private set; }
	}
}
