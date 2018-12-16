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
using Artemkv.Darwin.Controller.DataSources;
using Artemkv.Darwin.ERModel;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for EntityView.xaml
	/// </summary>
	public partial class RelationDetailsView : UserControl, IDetailsView
	{
		public RelationDetailsView()
		{
			InitializeComponent();

			ModalityComboBox.ItemsSource = Modality.GetModalities();
			CardinalityComboBox.ItemsSource = Cardinality.GetCardinalities();
		}

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var relation = value as RelationDTO;
				if (relation == null)
					throw new ArgumentNullException("value");

				this.DataContext = value;

				var serviceLocator = ServiceLocator.GetActive();

				var databaseID = new ProjectTreeHelper().GetFirstAncestorID<DatabaseDTO>();
				if (databaseID == Guid.Empty)
				{
					return;
				}

				ParentEntityComboBox.ItemsSource = serviceLocator.EntityInfo.GetEntities(databaseID);
				ForeignEntityComboBox.ItemsSource = new List<PersistentObjectDTO> 
				{ 
					new ObjectDataSource().GetObject(typeof(Entity), relation.ForeignEntityID) 
				};

				BindAttributesItemsSource();
			}
		}

		private void BindAttributesItemsSource(bool preservePrimaryColumnBindings = false)
		{
			var relation = DataContext as RelationDTO;
			if (relation == null)
			{
				return;
			}

			var serviceLocator = ServiceLocator.GetActive();

			if (!preservePrimaryColumnBindings)
			{
				PrimaryAttributeColumn.ItemsSource = serviceLocator.AttributeInfo.GetAttributes(relation.PrimaryEntityID);
			}
			ForeignAttributeColumn.ItemsSource = serviceLocator.AttributeInfo.GetAttributes(relation.ForeignEntityID);
		}

		private void ParentEntityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BindAttributesItemsSource();
		}

		private void EditForeignEntityButton_Click(object sender, RoutedEventArgs e)
		{
			var relation = Object as RelationDTO;
			if (relation == null || relation.ForeignEntityID == Guid.Empty)
			{
				return;
			}

			var entity = new ObjectDataSource().GetObject(typeof(Entity), relation.ForeignEntityID) as EntityDTO;

			var serviceLocator = ServiceLocator.GetActive();
			serviceLocator.ModelController.ProcessEditEntity(entity);
			BindAttributesItemsSource(preservePrimaryColumnBindings: true);
		}
	}
}
