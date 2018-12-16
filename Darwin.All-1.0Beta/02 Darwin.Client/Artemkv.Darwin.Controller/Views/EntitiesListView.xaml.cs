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
using Artemkv.Darwin.Common;
using Artemkv.Darwin.Controller.Filters;
using Artemkv.Darwin.Common.DTO;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for EntitiesListView.xaml
	/// </summary>
	public partial class EntitiesListView : UserControl, IListView
	{
		private IListDataSource _dataSource;

		public EntitiesListView()
		{
			InitializeComponent();
		}

		public IListDataSource DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				_dataSource = value;
				if (_dataSource == null)
					throw new ArgumentNullException("value");

				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					throw new InvalidOperationException("No database selected.");
				}
				var diagramID = new ProjectTreeHelper().GetFirstAncestorID<DiagramDTO>();
				if (diagramID == Guid.Empty)
				{
					throw new InvalidOperationException("No diagram selected.");
				}
				var filter = new EntitiesNotOnDiagramFilter(databaseID, diagramID);
				DataGridPager.BindToGridAndDataSource(EntitiesGrid, _dataSource, filter);
			}
		}

		public IEnumerable<ObjectListItem> SelectedItems
		{
			get
			{
				return (from x in EntitiesGrid.ItemsSource as IEnumerable<ObjectListItem> 
						where x.IsSelected select x).ToList<ObjectListItem>();
			}
		}

		private void SelectAllButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (ObjectListItem item in EntitiesGrid.Items)
			{
				item.IsSelected = true;
			}
		}

		private void SelectNoneButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (ObjectListItem item in EntitiesGrid.Items)
			{
				item.IsSelected = false;
			}
		}
	}
}
