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
using Artemkv.Darwin.Common.Requests;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for AttributeDetailsView.xaml
	/// </summary>
	public partial class DiagramEntityDetailsView : UserControl, IDetailsView
	{
		public DiagramEntityDetailsView()
		{
			InitializeComponent();
		}

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var entity = value as DiagramEntityDTO;
				if (entity == null)
					throw new ArgumentNullException("value");

				this.DataContext = value;

				var serviceLocator = ServiceLocator.GetActive();
				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				TypeComboBox.ItemsSource = serviceLocator.EntityInfo.GetEntities(databaseID);
			}
		}
	}
}
