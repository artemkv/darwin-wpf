using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Artemkv.Darwin.Controller.Areas;
using System.IO;

namespace Artemkv.Darwin.Controller
{
	/// <summary>
	/// Services and objects locator.
	/// See the pattern description at: <see cref="http://artemkondratyev.net/createstoreget.htm"/>.
	/// </summary>
	public class ServiceLocator
	{
		#region Class Members

		/// <summary>
		/// Registry to store objects.
		/// All objects that are stored in the registry are considered to be valid 
		/// and the same all time during the session. 
		/// </summary>
		private Registry _registry = null;

		/// <summary>
		/// The single instance.
		/// </summary>
		private static ServiceLocator _self = null;

		#endregion Class Members

		#region Constructors

		/// <summary>
		/// Initializes a new instance of ServiceLocator class.
		/// </summary>
		private ServiceLocator()
		{
			_registry = new Registry();
		}

		/// <summary>
		/// Performs static initialization of ServiceLocator class.
		/// </summary>
		static ServiceLocator()
		{
			_self = new ServiceLocator();
		}

		#endregion Constructors

		#region Static Methods

		/// <summary>
		/// Returns the active instance of ServiceLocator object.
		/// <example>
		/// This code is recommended to get the active instance:
		/// <c> ServiceLocator serviceLocator = ServiceLocator.GetActive(); </c>
		/// </example>
		/// </summary>
		/// <returns>The active instance of ServiceLocator object.</returns>
		public static ServiceLocator GetActive()
		{
			return _self;
		}

		#endregion Static Methods

		#region Public Properties

		public SessionState SessionState
		{
			get
			{
				if (_registry.SessionState == null)
				{
					_registry.SessionState = new SessionState();
				}

				return _registry.SessionState;
			}
		}

		public string LocalApplicationDataFolder
		{
			get
			{
				var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Darwin");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				return path;
			}
		}

		public BasicController BasicController
		{
			get
			{
				if (_registry.BasicController == null)
				{
					_registry.BasicController = new BasicController();
				}

				return _registry.BasicController;
			}
		}

		public ProjectController ProjectController
		{
			get
			{
				if (_registry.ProjectController == null)
				{
					_registry.ProjectController = new ProjectController();
				}

				return _registry.ProjectController;
			}
		}

		public ModelController ModelController
		{
			get
			{
				if (_registry.ModelController == null)
				{
					_registry.ModelController = new ModelController();
				}

				return _registry.ModelController;
			}
		}

		public ImportController ImportController
		{
			get
			{
				if (_registry.ImportController == null)
				{
					_registry.ImportController = new ImportController();
				}

				return _registry.ImportController;
			}
		}

		internal BaseEnumInfo BaseEnumInfo
		{
			get
			{
				if (_registry.BaseEnumInfo == null)
				{
					_registry.BaseEnumInfo = new BaseEnumInfo();
				}

				return _registry.BaseEnumInfo;
			}
		}

		internal DataTypeInfo DataTypeInfo
		{
			get
			{
				if (_registry.DataTypeInfo == null)
				{
					_registry.DataTypeInfo = new DataTypeInfo();
				}

				return _registry.DataTypeInfo;
			}
		}

		internal EntityInfo EntityInfo
		{
			get
			{
				if (_registry.EntityInfo == null)
				{
					_registry.EntityInfo = new EntityInfo();
				}

				return _registry.EntityInfo;
			}
		}

		internal AttributeInfo AttributeInfo
		{
			get
			{
				if (_registry.AttributeInfo == null)
				{
					_registry.AttributeInfo = new AttributeInfo();
				}

				return _registry.AttributeInfo;
			}
		}

		internal RelationInfo RelationInfo
		{
			get
			{
				if (_registry.RelationInfo == null)
				{
					_registry.RelationInfo = new RelationInfo();
				}

				return _registry.RelationInfo;
			}
		}

		internal DiagramEntityInfo DiagramEntityInfo
		{
			get
			{
				if (_registry.DiagramEntityInfo == null)
				{
					_registry.DiagramEntityInfo = new DiagramEntityInfo();
				}

				return _registry.DiagramEntityInfo;
			}
		}

		internal Paging Paging
		{
			get
			{
				if (_registry.Paging == null)
				{
					int pageSize = Paging.DefaultPageSize;
					var settings = ConfigurationManager.AppSettings["PageSize"];
					if (settings != null)
					{
						if (!Int32.TryParse(settings, out pageSize))
						{
							pageSize = Paging.DefaultPageSize;
						}
					}
					_registry.Paging = new Paging(pageSize);
				}

				return _registry.Paging;
			}
		}

		#endregion Public Properties
	}
}
