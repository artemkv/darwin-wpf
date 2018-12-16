using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.ERModel
{
	/// <summary>
	/// The modeling project.
	/// </summary>
	[Serializable]
	public class Project : PersistentObject
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the project name.
		/// </summary>
		public virtual string ProjectName { get; set; }

		#endregion Public Properties

		#region Public Methods

		public override string ToString()
		{
			return ProjectName;
		}

		#endregion Public Methods
	}
}
