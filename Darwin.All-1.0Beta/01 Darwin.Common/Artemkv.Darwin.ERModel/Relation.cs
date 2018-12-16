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
	public class Relation : PersistentObject
	{
		#region Class Members

		private readonly IList<RelationItem> _items = new List<RelationItem>();

		#endregion Class Members

		#region Public Properties

		/// <summary>
		/// Gets or sets the relation name.
		/// </summary>
		public virtual string RelationName { get; set; }

		/// <summary>
		/// The entity on the primary key side.
		/// </summary>
		public virtual Entity PrimaryEntity { get; set; }

		/// <summary>
		/// The entity on the foreign key side.
		/// </summary>
		public virtual Entity ForeignEntity { get; set; }

		/// <summary>
		/// The relation is 1:1 (as opposite to 1:M).
		/// </summary>
		public virtual bool OneToOne { get; set; }

		/// <summary>
		/// The relation requires at least one entity on the foreign key side.
		/// </summary>
		public virtual bool AtLeastOne { get; set; }

		/// <summary>
		/// Gets the collection of reltion items.
		/// </summary>
		public virtual IList<RelationItem> Items
		{
			get
			{
				return _items;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return RelationName;
		}

		#endregion Public Methods
	}
}
