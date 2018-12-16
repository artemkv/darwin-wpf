using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Artemkv.Darwin.Common.DTO;
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Controller.DataSources;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;

namespace Artemkv.Darwin.Controller
{
	/// <summary>
	/// Represents the tree node in a tree view.
	/// </summary>
	public class ObjectTreeNode : INotifyPropertyChanged
	{
		#region Private Members

		private ITreeDataSource _dataSource;

		#endregion Private Members

		#region .Ctors

		private ObjectTreeNode(bool createAsStub)
		{
			IsStub = createAsStub;
		}

		/// <summary>
		/// Creates a new tree node.
		/// </summary>
		/// <param name="parent">The parent node.</param>
		/// <param name="obj">The object which is represented by a tree node.</param>
		/// <param name="objectID">The object ID.</param>
		/// <param name="dataSource">The tree datasource.</param>
		/// <param name="path">The node path.</param>
		/// <param name="isLeaf">Specifies whether the node is a leaf.</param>
		public ObjectTreeNode(ObjectTreeNode parent, object obj, Guid objectID, ITreeDataSource dataSource, TreeNodePath path, bool isLeaf = false)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (dataSource == null)
				throw new ArgumentNullException("dataSource");
			if (path == null)
				throw new ArgumentNullException("path");

			this.Parent = parent;
			this.Object = obj;
			this.ObjectID = objectID;
			this._dataSource = dataSource;
			this.Path = path;
			this.IsLeaf = isLeaf;

			if (!IsLeaf)
			{
				Children = new ObservableCollection<ObjectTreeNode>() { new ObjectTreeNode(true) };
			}
		}

		#endregion .Ctors

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Events

		#region Private Properties

		private bool IsStub { get; set; }

		#endregion Private Properties

		#region Public Properties

		/// <summary>
		/// Gets the parent node.
		/// </summary>
		public ObjectTreeNode Parent { get; private set; }

		/// <summary>
		/// Gets the object represented by the tree node.
		/// </summary>
		public object Object { get; private set; }

		/// <summary>
		/// Gets or sets the object ID.
		/// </summary>
		public Guid ObjectID { get; private set; }

		/// <summary>
		/// Gets or sets the node path.
		/// </summary>
		public TreeNodePath Path { get; private set; }

		/// <summary>
		/// Specifies whether the node is a leaf.
		/// </summary>
		public bool IsLeaf { get; private set; }

		/// <summary>
		/// Gets the tree node children.
		/// </summary>
		public ObservableCollection<ObjectTreeNode> Children { get; private set; }

		// TODO:
				//Uri uri = new Uri
				//("pack://application:,,,/Images/diskdrive.png");
				//BitmapImage source = new BitmapImage(uri);
				//return source;
		public string IconSource 
		{
			get
			{
				if (Object is ProjectDTO)
				{
					return @"../Resources/Icons/Project16.ico";
				}
				if (Object is DatabaseDTO)
				{
					return @"../Resources/Icons/Database16.ico";
				}
				if (Object is EntityDTO)
				{
					return @"../Resources/Icons/Table16.ico";
				}
				var attr = Object as AttributeDTO;
				if (attr != null)
				{
					if (attr.IsPrimaryKey)
					{
						return @"../Resources/Icons/PrimaryKey16.ico";
					}
					else
					{
						return @"../Resources/Icons/Attribute16.ico";
					}
				}
				if (Object is ObjectGroup)
				{
					return @"../Resources/Icons/Folder16.ico";
				}
				if (Object is RelationDTO)
				{
					return @"../Resources/Icons/Relation16.ico";
				}
				if (Object is RelationItemDTO)
				{
					return @"../Resources/Icons/RelationItem16.ico";
				}
				if (Object is DiagramDTO)
				{
					return @"../Resources/Icons/Diagram16.ico";
				}
				if (Object is DiagramEntityDTO)
				{
					return @"../Resources/Icons/Table16.ico";
				}
				if (Object is BaseEnumDTO)
				{
					return @"../Resources/Icons/BaseEnum16.ico";
				}
				if (Object is BaseEnumValueDTO)
				{
					return @"../Resources/Icons/BaseEnumValue16.ico";
				}
				if (Object is DataTypeDTO)
				{
					return @"../Resources/Icons/DataType16.ico";
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the text to show as a tree node header.
		/// </summary>
		public string Text
		{
			get
			{
				if (IsStub)
				{
					return "?";
				}
				return Object.ToString();
			}
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Processes the node expand.
		/// </summary>
		public void OnExpand()
		{
			if (!IsLeaf && Children.Count > 0 && Children[0].IsStub)
			{
				ReloadChildren();
			}
		}

		/// <summary>
		/// Refreshes the node.
		/// </summary>
		public void Refresh()
		{
			var persistentObject = Object as PersistentObjectDTO;
			if (persistentObject != null)
			{
				Object = new ObjectDataSource().GetObject(persistentObject.PersistentType, persistentObject.ID);
				
				// Invalidate all properties
				OnPropertyChanged(new PropertyChangedEventArgs(null));
			}
			if (!IsLeaf)
			{
				ReloadChildren();
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, e);
			}
		}

		private void ReloadChildren()
		{
			var serviceLocator = ServiceLocator.GetActive();
			var busyIndicator = serviceLocator.SessionState.BusyIndicator;

			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += (sender, e) =>
			{
				e.Result = _dataSource.GetNodes(this);
			};
			worker.RunWorkerCompleted += (sender, e) =>
			{
				Children.Clear();

				if (busyIndicator != null)
				{
					lock (this)
					{
						busyIndicator.IsBusy = false;
						// Re-throw async exception.
						if (e.Error != null)
						{
							throw e.Error;
						}
					}
				}

				var children = e.Result as List<ObjectTreeNode>;
				if (children != null)
				{
					foreach (var child in children)
					{
						Children.Add(child);
					}
				}
			};
			worker.RunWorkerAsync();
			Thread.Sleep(100);
			lock (this)
			{
				if (busyIndicator != null && worker.IsBusy)
				{
					busyIndicator.IsBusy = true;
				}
			}
		}

		#endregion Private Methods
	}
}
