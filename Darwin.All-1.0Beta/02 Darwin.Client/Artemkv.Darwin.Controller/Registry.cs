using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemkv.Darwin.Controller.Areas;

namespace Artemkv.Darwin.Controller
{
	/// <summary>
	/// Long-living objects storage.
	/// See the pattern description at: <see cref="http://artemkondratyev.net/createstoreget.htm"/>.
	/// </summary>
	internal class Registry
	{
		public SessionState SessionState { get; set; }

		public BasicController BasicController { get; set; }
		public ProjectController ProjectController { get; set; }
		public ModelController ModelController { get; set; }
		public ImportController ImportController { get; set; }

		public BaseEnumInfo BaseEnumInfo { get; set; }
		public DataTypeInfo DataTypeInfo { get; set; }
		public EntityInfo EntityInfo { get; set; }
		public AttributeInfo AttributeInfo { get; set; }
		public RelationInfo RelationInfo { get; set; }
		public DiagramEntityInfo DiagramEntityInfo { get; set; }
		
		public Paging Paging { get; set; }
	}
}
