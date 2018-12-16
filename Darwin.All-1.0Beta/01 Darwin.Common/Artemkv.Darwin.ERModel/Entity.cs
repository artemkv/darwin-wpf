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
	public class Entity : PersistentObject
	{
		#region Class Members

		private readonly IList<ERModel.Attribute> _attributes = new List<ERModel.Attribute>();

		#endregion Class Members

		#region Public Properties

		/// <summary>
		/// Gets or sets the database.
		/// </summary>
		public virtual Database Database { get; set; }

		/// <summary>
		/// Gets or sets the schema name.
		/// </summary>
		public virtual string SchemaName { get; set; }

		/// <summary>
		/// Gets or sets the entity name.
		/// </summary>
		public virtual string EntityName { get; set; }

		/// <summary>
		/// Gets the collection of attributes.
		/// </summary>
		public virtual IList<ERModel.Attribute> Attributes
		{
			get
			{
				return _attributes;
			}
		}

		/// <summary>
		/// Gets the entity full name.
		/// </summary>
		public virtual string EntitySchemaPrefixedName
		{
			get
			{
				return SchemaName + "." + EntityName;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return EntitySchemaPrefixedName;
		}

		#endregion Public Methods
	}
}
