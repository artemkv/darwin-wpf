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

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for ProjectsView.xaml
	/// </summary>
	public partial class ProjectListView : UserControl, IListView
	{
		private IListDataSource _dataSource;

		public ProjectListView()
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

				DataGridPager.BindToGridAndDataSource(ProjectsGrid, _dataSource, ListFilter.Empty);
			}
		}

		public object SelectedItem
		{
			get
			{
				return ProjectsGrid.SelectedItem;
			}
		}

		private void CreateNewProjectButton_Click(object sender, RoutedEventArgs e)
		{
			ServiceLocator serviceLocator = ServiceLocator.GetActive();
			serviceLocator.ProjectController.ProcessCreateNewProject();

			DataGridPager.BindToGridAndDataSource(ProjectsGrid, _dataSource, ListFilter.Empty);
		}
	}
}
