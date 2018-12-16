using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for EntityView.xaml
	/// </summary>
	public partial class EntityDetailsView : UserControl, IDetailsView
	{
		public EntityDetailsView()
		{
			InitializeComponent();
		}

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var entity = value as EntityDTO;
				if (entity == null)
					throw new ArgumentNullException("value");

				this.DataContext = value;

				var serviceLocator = ServiceLocator.GetActive();

				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					return;
				}

				TypeColumn.ItemsSource = serviceLocator.DataTypeInfo.GetDataTypes(databaseID);
			}
		}
	}
}
