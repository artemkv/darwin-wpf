using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	/// <summary>
	/// The entity.
	/// </summary>
	[Serializable]
	public class DiagramEntity : PersistentObject
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the diagram.
		/// </summary>
		public virtual Diagram Diagram { get; set; }

		/// <summary>
		/// Gets or sets the entity which is represented by entity visual.
		/// </summary>
		public virtual Entity Entity { get; set; }

		/// <summary>
		/// Gets or sets the x-coordinate of the diagram entity rectangle.
		/// </summary>
		public virtual int X { get; set; }

		/// <summary>
		/// Gets or sets the y-coordinate of the diagram entity rectangle.
		/// </summary>
		public virtual int Y { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return Entity.EntityName;
		}

		#endregion Public Methods
	}
}
