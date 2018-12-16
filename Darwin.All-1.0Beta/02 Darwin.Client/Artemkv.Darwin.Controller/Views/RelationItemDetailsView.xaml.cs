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
	public partial class RelationItemDetailsView : UserControl, IDetailsView
	{
		public RelationItemDetailsView()
		{
			InitializeComponent();
		}

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var relationItem = value as RelationItemDTO;
				if (relationItem == null)
					throw new ArgumentNullException("value");

				this.DataContext = value;

				var serviceLocator = ServiceLocator.GetActive();
				var relation = new ProjectTreeHelper().GetFirstAncestor<RelationDTO>();
				PrimaryAttributeComboBox.ItemsSource = serviceLocator.AttributeInfo.GetAttributes(relation.PrimaryEntityID);
				ForeignAttributeComboBox.ItemsSource = serviceLocator.AttributeInfo.GetAttributes(relation.ForeignEntityID);
			}
		}
	}
}
