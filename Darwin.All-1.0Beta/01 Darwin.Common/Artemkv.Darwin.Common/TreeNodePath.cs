using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemkv.Darwin.Common
{
	/// <summary>
	/// Identifies the node path in the tree.
	/// </summary>
	public class TreeNodePath
	{
		#region Class Members

		private string _path;
		private string _leaf;

		#endregion Class Members

		#region .Ctors

		private TreeNodePath(string basePath)
		{
			_path = basePath;
			_leaf = String.Empty;
		}

		private TreeNodePath(string basePath, string nextElement)
		{
			_path = basePath + "|" + nextElement;
			_leaf = nextElement;
		}

		#endregion .Ctors

		#region Operators

		public static implicit operator String(TreeNodePath path)
		{
			return path.ToString();
		}

		#endregion Operators

		#region Public Properties

		public string Leaf
		{
			get
			{
				return _leaf;
			}
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Returns the root path.
		/// </summary>
		public static TreeNodePath Root
		{
			get
			{
				return new TreeNodePath(".");
			}
		}

		/// <summary>
		/// Returns the new path as combination of the current path and a next element.
		/// </summary>
		/// <param name="nextElement">The next element.</param>
		/// <returns>The new path.</returns>
		public TreeNodePath Then(string nextElement)
		{
			return new TreeNodePath(_path, nextElement);
		}

		public override string ToString()
		{
			return _path;
		}

		#endregion Public Methods
	}
}
