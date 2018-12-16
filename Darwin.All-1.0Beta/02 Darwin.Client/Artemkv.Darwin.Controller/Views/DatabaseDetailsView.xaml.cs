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
	/// Interaction logic for DatabaseDetailsView.xaml
	/// </summary>
	public partial class DatabaseDetailsView : UserControl, IDetailsView
	{
		public DatabaseDetailsView()
		{
			InitializeComponent();
		}

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var database = value as DatabaseDTO;
				if (database == null)
					throw new ArgumentNullException("value");

				this.DataContext = value;

				var serviceLocator = ServiceLocator.GetActive();

				BaseEnumColumn.ItemsSource = serviceLocator.BaseEnumInfo.GetBaseEnums(database.ID);
			}
		}
	}
}
