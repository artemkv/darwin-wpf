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

namespace Darwin
{
	/// <summary>
	/// Interaction logic for ExceptionView.xaml
	/// </summary>
	public partial class ExceptionWindow : Window
	{
		public ExceptionWindow()
		{
			InitializeComponent();
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			this.Close();
		}

		public string Message
		{
			get
			{
				return ExceptionMessage.Text;
			}
			set
			{
				ExceptionMessage.Text = value;
			}
		}

		public string Details
		{
			get
			{
				return ExceptionDetails.Text;
			}
			set
			{
				ExceptionDetails.Text = value;
			}
		}
	}
}
