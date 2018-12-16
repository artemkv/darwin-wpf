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
using Artemkv.Darwin.Visual.Primitives;
using Artemkv.Darwin.Visual.Connectors;
using System.Windows.Media.Effects;
using Artemkv.Darwin.Visual.Layout;

namespace Artemkv.Darwin.Controller.Views
{
	/// <summary>
	/// Interaction logic for EntityView.xaml
	/// </summary>
	public partial class DiagramDetailsView : UserControl, IDetailsView
	{
		#region .Ctor

		public DiagramDetailsView()
		{
			InitializeComponent();

			PaperSizeComboBox.ItemsSource = PaperSize.GetSizes();
			PaperOrientationComboBox.ItemsSource = PaperOrientation.GetOrientations();
		}

		#endregion .Ctor

		#region IDetailsView Members

		public PersistentObjectDTO Object
		{
			get { return this.DataContext as PersistentObjectDTO; }
			set
			{
				var diagram = value as DiagramDTO;
				if (diagram == null)
					throw new ArgumentNullException("value");

				if (diagram.PaperSize == 0)
				{
					diagram.PaperSize = PaperSize.A4;
				}

				this.DataContext = value;
			}
		}

		#endregion IDetailsView Members
	}
}
