using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	[Serializable]
	public class Diagram : PersistentObject
	{
		#region Class Members

		private readonly IList<DiagramEntity> _entities = new List<DiagramEntity>();

		#endregion Class Members

		#region Public Properties

		/// <summary>
		/// Gets or sets the database.
		/// </summary>
		public virtual Database Database { get; set; }

		/// <summary>
		/// Gets or sets the diagram name.
		/// </summary>
		public virtual string DiagramName { get; set; }

		/// <summary>
		/// Gets or sets the paper size.
		/// </summary>
		public virtual int PaperSize { get; set; }

		/// <summary>
		/// Gets or sets the value which specifies the paper orientation.
		/// </summary>
		public virtual bool IsVertical { get; set; }

		/// <summary>
		/// Gets or sets the value which specifies whether the diagram objects were adjusted by the user.
		/// </summary>
		public virtual bool IsAdjusted { get; set; }

		/// <summary>
		/// Gets or sets the value which specifies whether the modality should be shown on a diagram.
		/// </summary>
		public virtual bool ShowModality { get; set; }

		/// <summary>
		/// Gets the collection of diagram entitites.
		/// </summary>
		public virtual IList<DiagramEntity> Entities
		{
			get
			{
				return _entities;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return DiagramName;
		}

		#endregion Public Methods
	}
}
