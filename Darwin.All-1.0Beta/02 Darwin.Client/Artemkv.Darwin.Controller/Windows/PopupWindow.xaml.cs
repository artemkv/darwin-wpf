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

using DarwinValidation = Artemkv.Darwin.Common.Validation;

namespace Artemkv.Darwin.Controller.Windows
{
	/// <summary>
	/// Interaction logic for PopupWindow.xaml
	/// </summary>
	public partial class PopupWindow : Window
	{
		public PopupWindow()
		{
			InitializeComponent();
		}

		public DockPanel ViewPanel
		{
			get
			{
				return InnerViewPanel;
			}
		}

		public Func<DarwinValidation.ValidationError> Validate { get; set; }

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			DarwinValidation.ValidationError error = null;
			if (Validate != null)
			{
				error = Validate();
			}

			if (error == null)
			{
				DialogResult = true;
				this.Close();
			}
			else
			{
				MessageBox.Show(error.ToString(), "Validation Error", MessageBoxButton.OK);
			}
		}
	}
}
