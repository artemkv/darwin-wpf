using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	[Serializable]
	public class Database : PersistentObject
	{
		#region Class Members

		private readonly IList<DataType> _dataTypes = new List<DataType>();

		#endregion Class Members

		#region Public Properties

		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		public virtual Project Project { get; set; }

		/// <summary>
		/// Gets or sets the database name.
		/// </summary>
		public virtual string DBName { get; set; }

		/// <summary>
		/// Gets or sets the database connection string.
		/// </summary>
		public virtual string ConnectionString { get; set; }

		/// <summary>
		/// Gets the collection of data types.
		/// </summary>
		public virtual IList<DataType> DataTypes
		{
			get
			{
				return _dataTypes;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return DBName;
		}

		#endregion Public Methods
	}
}
